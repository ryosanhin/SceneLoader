using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneLoader : MonoBehaviour
{
    #region singleton
    private static SceneLoader _instance;

    public static SceneLoader instance{
		get{return _instance;}
	}
	
    private void Awake()
    {
        if (_instance!=null && _instance!=this){
            Destroy(this.gameObject);
        }else{
            _instance=this;
			DontDestroyOnLoad(this.gameObject);
        }
    }
	#endregion
	
	public int state=0;
	/*
	0:don't working
	1:now loading
	2:finished loading
	*/
	
	public float transitionTime=1f;
	public Material mate;
	int id=0;
	[SerializeField] string propertyName="_Alpha";
	[SerializeField] string UILayerName;
	int maskNum=0;
	
	Camera myOverlayCamera;
	
	void Start(){
		//マテリアルのプロパティ取得、一応透明に
		id=Shader.PropertyToID(propertyName);
		mate.SetFloat(id,0f);
		
		//遷移用UI用のレイヤー番号を取得
		maskNum=LayerMask.NameToLayer(UILayerName);
		
		//遷移用UI用のカメラを取得
		myOverlayCamera=transform.Find("TransitionCamera").GetComponent<Camera>();
		//このカメラのマスクを変更
		myOverlayCamera.cullingMask=1<<maskNum;
		//遷移後もう一度メインカメラにスタックするように関数を登録
		SceneManager.sceneLoaded+=SceneLoaded;
		
		SetTransitionCamera();
	}
	
    public void LoadScene(string name){
		StartCoroutine(SceneLoading(name));
	}
	
	IEnumerator SceneLoading(string name){
		if(state>0){
			yield break;
		}else{
			state=1;
		}
		
		AsyncOperation _async; //非同期でシーンをロード
		Material m=mate;
		
		/*ここに画面遷移エフェクトを入れる*/
		yield return StartCoroutine(TransitionAnima(m,id,1));
		/*ここに画面遷移エフェクトを入れる*/
		
		/*シーンをロード*/
		_async=SceneManager.LoadSceneAsync(name);
		_async.allowSceneActivation=false;
		
		float waitTime=0f;
		
		while(_async.progress<0.9f || waitTime<0.5f){
			waitTime+=Time.deltaTime;
			yield return null;
		}
		
		/*シーンを実際にロード*/
		_async.allowSceneActivation=true;
		state=2;
		
		/*ここに画面遷移エフェクトを入れる*/
		yield return StartCoroutine(TransitionAnima(m,id,0));
		/*ここに画面遷移エフェクトを入れる*/
		
		state=0;
	}
	
	IEnumerator TransitionAnima(Material m,int mateID,int to){
		float frame=transitionTime*60f;
		float inverse=0f;
		if(frame>0f){
			inverse=1f/frame;
		}
		
		//0to1->隠す
		if(to==1){
			for(float f=0;f<frame;f++){
				m.SetFloat(mateID,f*inverse);
				yield return null;
			}
			m.SetFloat(mateID,1f);
		}
		
		//1to0->外す
		if(to==0){
			for(float f=frame;f>0f;f--){
				m.SetFloat(mateID,f*inverse);
				yield return null;
			}
			m.SetFloat(mateID,0f);
		}
		
		yield return null;
	}
	
	void SceneLoaded (Scene nextScene, LoadSceneMode mode) {
		SetTransitionCamera();
    }
	
	void SetTransitionCamera(){
		//メインカメラのマスクを変更
		Camera.main.cullingMask=~(1<<maskNum);
		//メインカメラのURP用のスクリプト取得
		var cameraData=Camera.main.GetComponent<UniversalAdditionalCameraData>();
		//スタックカメラに追加
		cameraData.cameraStack.Add(myOverlayCamera);
	}
}
