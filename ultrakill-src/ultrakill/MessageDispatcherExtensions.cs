using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B6 RID: 438
public static class MessageDispatcherExtensions
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x0003A6B8 File Offset: 0x000388B8
	public static void RemoveAllListeners<TMessage>(this GameObject gameObject) where TMessage : MessageDispatcherBase
	{
		TMessage tmessage;
		if (!gameObject.TryGetComponent<TMessage>(out tmessage))
		{
			return;
		}
		tmessage.RemoveAllListeners();
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0003A6DB File Offset: 0x000388DB
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction handler) where TMessage : MessageDispatcher
	{
		gameObject.GetOrAddComponent<TMessage>().AddListener(handler);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0003A6EE File Offset: 0x000388EE
	public static void AddListener<TMessage, T>(this GameObject gameObject, UnityAction<T> handler) where TMessage : MessageDispatcher<T>
	{
		gameObject.GetOrAddComponent<TMessage>().AddListener(handler);
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0003A701 File Offset: 0x00038901
	public static void AddListener<TMessage, T1, T2>(this GameObject gameObject, UnityAction<T1, T2> handler) where TMessage : MessageDispatcher<T1, T2>
	{
		gameObject.GetOrAddComponent<TMessage>().AddListener(handler);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0003A714 File Offset: 0x00038914
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<int> handler) where TMessage : MessageDispatcher<int>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0003A71D File Offset: 0x0003891D
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<bool> handler) where TMessage : MessageDispatcher<bool>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003A726 File Offset: 0x00038926
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<global::AnimationEvent> handler) where TMessage : MessageDispatcher<global::AnimationEvent>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0003A72F File Offset: 0x0003892F
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<float[], int> handler) where TMessage : MessageDispatcher<float[], int>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0003A738 File Offset: 0x00038938
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<Collision> handler) where TMessage : MessageDispatcher<Collision>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0003A741 File Offset: 0x00038941
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<Collision2D> handler) where TMessage : MessageDispatcher<Collision2D>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0003A74A File Offset: 0x0003894A
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<ControllerColliderHit> handler) where TMessage : MessageDispatcher<ControllerColliderHit>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0003A753 File Offset: 0x00038953
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<float> handler) where TMessage : MessageDispatcher<float>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0003A75C File Offset: 0x0003895C
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<Joint2D> handler) where TMessage : MessageDispatcher<Joint2D>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0003A765 File Offset: 0x00038965
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<GameObject> handler) where TMessage : MessageDispatcher<GameObject>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0003A76E File Offset: 0x0003896E
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<RenderTexture, RenderTexture> handler) where TMessage : MessageDispatcher<RenderTexture, RenderTexture>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0003A777 File Offset: 0x00038977
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<Collider> handler) where TMessage : MessageDispatcher<Collider>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0003A780 File Offset: 0x00038980
	public static void AddListener<TMessage>(this GameObject gameObject, UnityAction<Collider2D> handler) where TMessage : MessageDispatcher<Collider2D>
	{
		gameObject.AddListener(handler);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0003A789 File Offset: 0x00038989
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction handler) where TMessage : MessageDispatcher
	{
		gameObject.GetOrAddComponent<TMessage>().RemoveListener(handler);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0003A79C File Offset: 0x0003899C
	public static void RemoveListener<TMessage, T>(this GameObject gameObject, UnityAction<T> handler) where TMessage : MessageDispatcher<T>
	{
		gameObject.GetOrAddComponent<TMessage>().RemoveListener(handler);
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0003A7AF File Offset: 0x000389AF
	public static void RemoveListener<TMessage, T1, T2>(this GameObject gameObject, UnityAction<T1, T2> handler) where TMessage : MessageDispatcher<T1, T2>
	{
		gameObject.GetOrAddComponent<TMessage>().RemoveListener(handler);
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0003A7C2 File Offset: 0x000389C2
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<int> handler) where TMessage : MessageDispatcher<int>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0003A7CB File Offset: 0x000389CB
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<bool> handler) where TMessage : MessageDispatcher<bool>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0003A7D4 File Offset: 0x000389D4
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<global::AnimationEvent> handler) where TMessage : MessageDispatcher<global::AnimationEvent>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x0003A7DD File Offset: 0x000389DD
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<float[], int> handler) where TMessage : MessageDispatcher<float[], int>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0003A7E6 File Offset: 0x000389E6
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<Collision> handler) where TMessage : MessageDispatcher<Collision>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0003A7EF File Offset: 0x000389EF
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<Collision2D> handler) where TMessage : MessageDispatcher<Collision2D>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0003A7F8 File Offset: 0x000389F8
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<ControllerColliderHit> handler) where TMessage : MessageDispatcher<ControllerColliderHit>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0003A801 File Offset: 0x00038A01
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<float> handler) where TMessage : MessageDispatcher<float>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0003A80A File Offset: 0x00038A0A
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<Joint2D> handler) where TMessage : MessageDispatcher<Joint2D>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0003A813 File Offset: 0x00038A13
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<GameObject> handler) where TMessage : MessageDispatcher<GameObject>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0003A81C File Offset: 0x00038A1C
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<RenderTexture, RenderTexture> handler) where TMessage : MessageDispatcher<RenderTexture, RenderTexture>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0003A825 File Offset: 0x00038A25
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<Collider> handler) where TMessage : MessageDispatcher<Collider>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0003A82E File Offset: 0x00038A2E
	public static void RemoveListener<TMessage>(this GameObject gameObject, UnityAction<Collider2D> handler) where TMessage : MessageDispatcher<Collider2D>
	{
		gameObject.RemoveListener(handler);
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0003A837 File Offset: 0x00038A37
	public static void RemoveAllListeners<TMessage>(this Component component) where TMessage : MessageDispatcherBase
	{
		component.gameObject.RemoveAllListeners<TMessage>();
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0003A844 File Offset: 0x00038A44
	public static void AddListener<TMessage>(this Component component, UnityAction handler) where TMessage : MessageDispatcher
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003A852 File Offset: 0x00038A52
	public static void AddListener<TMessage, T>(this Component component, UnityAction<T> handler) where TMessage : MessageDispatcher<T>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003A860 File Offset: 0x00038A60
	public static void AddListener<TMessage, T1, T2>(this Component component, UnityAction<T1, T2> handler) where TMessage : MessageDispatcher<T1, T2>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0003A86E File Offset: 0x00038A6E
	public static void AddListener<TMessage>(this Component component, UnityAction<int> handler) where TMessage : MessageDispatcher<int>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0003A87C File Offset: 0x00038A7C
	public static void AddListener<TMessage>(this Component component, UnityAction<bool> handler) where TMessage : MessageDispatcher<bool>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0003A88A File Offset: 0x00038A8A
	public static void AddListener<TMessage>(this Component component, UnityAction<global::AnimationEvent> handler) where TMessage : MessageDispatcher<global::AnimationEvent>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003A898 File Offset: 0x00038A98
	public static void AddListener<TMessage>(this Component component, UnityAction<float[], int> handler) where TMessage : MessageDispatcher<float[], int>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003A8A6 File Offset: 0x00038AA6
	public static void AddListener<TMessage>(this Component component, UnityAction<Collision> handler) where TMessage : MessageDispatcher<Collision>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0003A8B4 File Offset: 0x00038AB4
	public static void AddListener<TMessage>(this Component component, UnityAction<Collision2D> handler) where TMessage : MessageDispatcher<Collision2D>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003A8C2 File Offset: 0x00038AC2
	public static void AddListener<TMessage>(this Component component, UnityAction<ControllerColliderHit> handler) where TMessage : MessageDispatcher<ControllerColliderHit>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0003A8D0 File Offset: 0x00038AD0
	public static void AddListener<TMessage>(this Component component, UnityAction<float> handler) where TMessage : MessageDispatcher<float>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0003A8DE File Offset: 0x00038ADE
	public static void AddListener<TMessage>(this Component component, UnityAction<Joint2D> handler) where TMessage : MessageDispatcher<Joint2D>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003A8EC File Offset: 0x00038AEC
	public static void AddListener<TMessage>(this Component component, UnityAction<GameObject> handler) where TMessage : MessageDispatcher<GameObject>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0003A8FA File Offset: 0x00038AFA
	public static void AddListener<TMessage>(this Component component, UnityAction<RenderTexture, RenderTexture> handler) where TMessage : MessageDispatcher<RenderTexture, RenderTexture>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003A908 File Offset: 0x00038B08
	public static void AddListener<TMessage>(this Component component, UnityAction<Collider> handler) where TMessage : MessageDispatcher<Collider>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003A916 File Offset: 0x00038B16
	public static void AddListener<TMessage>(this Component component, UnityAction<Collider2D> handler) where TMessage : MessageDispatcher<Collider2D>
	{
		component.gameObject.AddListener(handler);
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003A924 File Offset: 0x00038B24
	public static void RemoveListener<TMessage>(this Component component, UnityAction handler) where TMessage : MessageDispatcher
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003A932 File Offset: 0x00038B32
	public static void RemoveListener<TMessage>(this Component component, UnityAction<int> handler) where TMessage : MessageDispatcher<int>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0003A940 File Offset: 0x00038B40
	public static void RemoveListener<TMessage>(this Component component, UnityAction<bool> handler) where TMessage : MessageDispatcher<bool>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003A94E File Offset: 0x00038B4E
	public static void RemoveListener<TMessage>(this Component component, UnityAction<global::AnimationEvent> handler) where TMessage : MessageDispatcher<global::AnimationEvent>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0003A95C File Offset: 0x00038B5C
	public static void RemoveListener<TMessage>(this Component component, UnityAction<float[], int> handler) where TMessage : MessageDispatcher<float[], int>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0003A96A File Offset: 0x00038B6A
	public static void RemoveListener<TMessage>(this Component component, UnityAction<Collision> handler) where TMessage : MessageDispatcher<Collision>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0003A978 File Offset: 0x00038B78
	public static void RemoveListener<TMessage>(this Component component, UnityAction<Collision2D> handler) where TMessage : MessageDispatcher<Collision2D>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0003A986 File Offset: 0x00038B86
	public static void RemoveListener<TMessage>(this Component component, UnityAction<ControllerColliderHit> handler) where TMessage : MessageDispatcher<ControllerColliderHit>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003A994 File Offset: 0x00038B94
	public static void RemoveListener<TMessage>(this Component component, UnityAction<float> handler) where TMessage : MessageDispatcher<float>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0003A9A2 File Offset: 0x00038BA2
	public static void RemoveListener<TMessage>(this Component component, UnityAction<Joint2D> handler) where TMessage : MessageDispatcher<Joint2D>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003A9B0 File Offset: 0x00038BB0
	public static void RemoveListener<TMessage>(this Component component, UnityAction<GameObject> handler) where TMessage : MessageDispatcher<GameObject>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003A9BE File Offset: 0x00038BBE
	public static void RemoveListener<TMessage>(this Component component, UnityAction<RenderTexture, RenderTexture> handler) where TMessage : MessageDispatcher<RenderTexture, RenderTexture>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003A9CC File Offset: 0x00038BCC
	public static void RemoveListener<TMessage>(this Component component, UnityAction<Collider> handler) where TMessage : MessageDispatcher<Collider>
	{
		component.gameObject.RemoveListener(handler);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0003A9DA File Offset: 0x00038BDA
	public static void RemoveListener<TMessage>(this Component component, UnityAction<Collider2D> handler) where TMessage : MessageDispatcher<Collider2D>
	{
		component.gameObject.RemoveListener(handler);
	}
}
