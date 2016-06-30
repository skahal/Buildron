#region Usings
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Xml;
using Skahal.Logging;
using Skahal.Common;
using System.Collections.Generic;
using Buildron.Infrastructure;
using Skahal.Threading;
#endregion

public class Requester : MonoBehaviour
{
	#region Constants
	public const float WaitForSecondsBetweenRequests = 0.1f;
	#endregion

	#region Events
	public event EventHandler<RequestFailedEventArgs> GetFailed;
	#endregion

	#region Fields
	private Queue<Action> m_requestsImmediatelyQueue = new Queue<Action>();
	private Queue<Action> m_requestsQueue = new Queue<Action>();
	#endregion

	#region Constructors
	static Requester ()
	{
		Instance = new GameObject ("BuildronRequester").AddComponent<Requester> ();
	}
	#endregion		

	#region Properties
	public static Requester Instance { get; private set; }
	public bool AcceptLanguageEnabled { get; set; }
	#endregion

	#region Methods
	private void Awake ()
	{
		StartCoroutine (DequeueRequests ());	
	}

	private IEnumerator DequeueRequests ()
	{
		while (true) {

			if (m_requestsImmediatelyQueue.Count == 0) {
				if (m_requestsQueue.Count > 0) {
					m_requestsQueue.Dequeue () ();
				}
			} else {
				m_requestsImmediatelyQueue.Dequeue () ();
			}

			// Be polite with the CI Server ;)
			yield return new WaitForSeconds(WaitForSecondsBetweenRequests);		
		}
	}

	public void Get (string url, Action<XmlDocument> responseReceived, Action errorReceived = null)
	{
		m_requestsQueue.Enqueue (() =>
			{
				StartCoroutine (DoGet (url, responseReceived, errorReceived));
			});
	}

	public void GetImmediately (string url, Action<XmlDocument> responseReceived, Action errorReceived = null)
	{
		m_requestsImmediatelyQueue.Enqueue (() =>
			{
				StartCoroutine (DoGet (url, responseReceived, errorReceived));
			});
	}


	public void GetText (string url, Action<string> responseReceived, Action errorReceived = null)
	{
		m_requestsQueue.Enqueue (() =>
			{
				StartCoroutine (DoGet (url, responseReceived, errorReceived));
			});
	}

	public void PostText (string url, Dictionary<string,string> fields,  Action<string> responseReceived, Action errorReceived = null)
	{
		m_requestsQueue.Enqueue (() =>
			{
				StartCoroutine (DoPost (url, fields, responseReceived, errorReceived));
			});
	}

	private IEnumerator DoPost (string url, Dictionary<string,string> fields, Action<string> responseReceived, Action errorReceived = null)
	{
		return DoBasicGet (url, (response) => {
			responseReceived (response.text);
		}, errorReceived, fields);
	}

	public void GetTextImmediately (string url, Action<string> responseReceived, Action errorReceived = null)
	{
		m_requestsImmediatelyQueue.Enqueue (() =>
			{
				StartCoroutine (DoGet (url, responseReceived, errorReceived));
			});
	}


	public void Request (string url)
	{
		m_requestsQueue.Enqueue (() =>
			{
				StartCoroutine (DoBasicGet (url, null));
			});
	}

	public void RequestImmediately (string url)
	{
		m_requestsImmediatelyQueue.Enqueue (() =>
			{
				StartCoroutine (DoBasicGet (url, null));
			});
	}

	public void GetTexture (string url, Action<Texture2D> responseReceived, Action errorReceived = null)
	{
		m_requestsQueue.Enqueue (() =>
			{
				StartCoroutine (DoGet (url, responseReceived, errorReceived));
			});
	}

	private IEnumerator DoGet (string url, Action<XmlDocument> responseReceived, Action errorReceived = null)
	{
		return DoBasicGet (url, (response) => {
			var doc = new XmlDocument ();
			doc.LoadXml (response.text);
			responseReceived (doc);
		}, errorReceived);
	}

	private IEnumerator DoGet (string url, Action<string> responseReceived, Action errorReceived = null)
	{
		return DoBasicGet (url, (response) => {
			responseReceived (response.text);
		}, errorReceived);
	}

	private IEnumerator DoGet (string url, Action<Texture2D> responseReceived, Action errorReceived = null)
	{
		return DoBasicGet (url, (response) => {
			responseReceived (response.texture);
		},
			errorReceived);	
	}

	private IEnumerator DoBasicGet (string url, Action<WWW> responseReceived)
	{
		return DoBasicGet(url, responseReceived, null, null);
	}

	private IEnumerator DoBasicGet (string url, Action<WWW> responseReceived, Action errorReceived, Dictionary<string, string> fields = null)
	{
		SHLog.Debug ("Requesting URL '{0}' on the server...", url);
		WWW request;

		if (AcceptLanguageEnabled) {
			var form = new WWWForm ();
			form.AddField ("Buildron", SHGameInfo.Version);

			var headers = form.headers;
			var rawData = form.data;
			headers ["Accept-Language"] = "en-US";

			request = new WWW (url, rawData, headers);
		} if (fields != null) {
			var form = new WWWForm ();

			foreach(var f in fields)
			{
				form.AddField(f.Key, f.Value);
			}

			request = new WWW (url, form);
		}
		else {
			request = new WWW (url);
		}

		// Timeout
		bool hasTimeout = false;
		SHThread.Start(
			60, 
			() =>
			{
				if(request != null && !request.isDone)
				{
					try
					{
						SHLog.Warning("Request timeout for url {0}.", url);
						hasTimeout = true;
						request.Dispose();
					}
					catch(ObjectDisposedException)
					{
						SHLog.Warning("Request already disposed.", url);
						return;
					}
				}
			});

		yield return request;

		if (hasTimeout)
		{
			if (errorReceived != null)
			{
				errorReceived();
			}
			else
			{
				GetFailed.Raise(this, new RequestFailedEventArgs(url));
			}            
		}
		else
		{
			var status = request.responseHeaders.ContainsKey("STATUS") ? request.responseHeaders["STATUS"] : "";

			if (request.error == null
				&& !status.Equals("HTTP/1.0 302 Found", StringComparison.InvariantCultureIgnoreCase)
				&& !request.text.Contains("Status Code: 404"))
			{
				SHLog.Debug("Response from server to URL '{0}': {1}", url, request.text);

				if (responseReceived != null)
				{
                    try
                    {
                        responseReceived(request);
                    }
                    catch(Exception ex)
                    {
                        SHLog.Warning("Error calling responseReceived for url '{0}': {1}. {2}", url, ex.Message, ex.StackTrace);
                        GetFailed.Raise(this, new RequestFailedEventArgs(url));
                    }
				}

			}
			else {
				SHLog.Warning("Error from server to URL '{0}': {1}", url, request.error);

				if (errorReceived != null)
				{
					errorReceived();
				}
				else if (request.error == null || (!request.error.Contains("404 Not Found") && !request.text.Contains("Status Code: 404")))
				{
					SHLog.Warning("Error requesting URL '{0}': {1}", url, request.error == null ? status : request.error);
					GetFailed.Raise(this, new RequestFailedEventArgs(url));
				}
			}
		}
	}
	#endregion
}
