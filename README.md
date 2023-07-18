# SceneLoader
Scene loader for Unity. Unity向けの簡易シーン遷移ツールです。  
# 使い方
1. SceneLoaderフォルダ内のSceneLoaderプレハブをシーンに配置します。
2. シーン遷移エフェクトUI用のレイヤー名を決め、SceneLoader以下すべてに適用します。  
   また、シーン内のSceneLoaderにアタッチされているSceneLoader.csのUILayerNameを先ほど決めたレイヤー名に変更してください。  
   注意：UILayerNameに入力されたレイヤーはメインカメラには映らなくなります。  
![InputLayerName](https://github.com/ryosanhin/SceneLoader-forUnity/assets/90621212/3287e242-a35c-4f78-baa3-395a176a2102)
![SelectLayerName](https://github.com/ryosanhin/SceneLoader-forUnity/assets/90621212/e0217f49-a682-4a8d-8b8e-0114be94eea9)
3. これで使用する用意は整いました。スクリプトから呼び出してシーン遷移を彩りましょう。  
![Example](https://github.com/ryosanhin/SceneLoader-forUnity/assets/90621212/bb96a19c-acd1-42fa-9357-dc94f04e8a9f)
4. マテリアルや遷移時の画像については、[マテリアル](#マテリアル)を参照してください。
# スクリプト
SceneLoaderクラスはシングルトンパターンを利用しています。
### 関数
public LoadScene(string name);
```
//hogehogeという名前のシーンを読み込み
SceneLoader.instance.LoadScene("hogehoge");
```
### 変数
```
//ロードの状態を表す。
//state=0 動作なし、state=1 読み込み中、state=2読み込み完了。
//読み込みが完全に終了後 state は0に戻ります。
public int state=0;

//シーン遷移エフェクトにかかる時間
public float transitionTime=1f;

//シーン遷移エフェクト用のマテリアル
public Material mate;

//シェーダーのプロパティの名前
[SerializeField] string propertyName="_Alpha";

//シーン遷移エフェクトUI用のレイヤー名
[SerializeField] string UILayerName;
```
# マテリアル
同時に封入されているマテリアルはシーン遷移用になっています。  
グレースケールを使ってアルファを変更しているので、もし違うエフェクトを付けたい場合は  
グレースケールで作られている画像を入手してください。  
遷移エフェクトを変更する場合はマテリアルのTransitionTexにエフェクト用のグレースケール画像をアタッチしてください。  
![Material](https://github.com/ryosanhin/SceneLoader-forUnity/assets/90621212/389053d0-17a1-4650-a69a-354232d1c636)  
  
遷移時に表示される画像を変更したい場合は、  
シーンに配置したSceneLoader/TransitionCanvas/TransitionScreenのImageコンポーネントのソース画像を変更してください。  
![ChangeImage](https://github.com/ryosanhin/SceneLoader-forUnity/assets/90621212/f65a68ad-f7ad-4407-9a98-f8f8e6662082)
  
  
