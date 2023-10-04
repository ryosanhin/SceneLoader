using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	
	bool isLoading=false;
	
	public float transitionTime=1f;
	public float defaultLoadingTime=0.5f;
	public Material mate;
	[SerializeField] GraphicRaycaster gr;
	int id=0;
	string propertyName="_Alpha";
	
	void Start(){
		//マテリアルのプロパティ取得、一応透明に
		id=Shader.PropertyToID(propertyName);
		mate.SetFloat(id,0f);
		gr.enabled=false;
	}
	
	public void LoadScene(int i){
		StartCoroutine(SceneLoading(i));
	}
	
    public void LoadScene(string name){
		var scene=SceneManager.GetSceneByName(name);
		StartCoroutine(SceneLoading(scene.buildIndex));
	}
	
	IEnumerator SceneLoading(int num){
		if(isLoading)yield break;
		isLoading=true;
		
		gr.enabled=true;
		
		/*ここに画面遷移エフェクトを入れる*/
		yield return StartCoroutine(TransitionAnima(1));
		/*ここに画面遷移エフェクトを入れる*/
		
		/*シーンをロード*/
		AsyncOperation _async=SceneManager.LoadSceneAsync(num);
		_async.allowSceneActivation=false;
		
		float waitTime=0f;
		
		while(_async.progress<0.9f || waitTime<defaultLoadingTime){
			waitTime+=Time.deltaTime;
			yield return null;
		}
		
		/*シーンを実際にロード*/
		_async.allowSceneActivation=true;
		
		/*ここに画面遷移エフェクトを入れる*/
		yield return StartCoroutine(TransitionAnima(0));
		/*ここに画面遷移エフェクトを入れる*/
		
		isLoading=false;
		gr.enabled=false;
	}
	
	public IEnumerator TransitionAnima(int to){
		float inverse=1f/transitionTime;
		float f=0f;
		float time=0f;
		while(time<transitionTime){
			time+=Time.deltaTime;
			switch(to){
				case 0:
				//1to0->外す
				f=1f-time*inverse;
				break;
				case 1:
				//0to1->隠す
				f=time*inverse;
				break;
			}
			mate.SetFloat(id,f);
			yield return null;
		}
		mate.SetFloat(id,to);
		
		yield return null;
	}
}
