# Shooting2403

シューティングゲームの一連の流れを実装したものになります

Scene
- Title
- Main(シューティング)
- Score

操作説明
- Title
   -  真ん中に表示されているボタンをクリックすると始まります
- Main
  - 敵や敵の攻撃を避けながら攻撃します
     敵を撃破するとスコアが貯まります
     敵と接触するor敵の弾に3回当たるor30秒経過で終了です
  - 自機
    - ←キー　→キー　で移動
    - Spaceキーで弾発射
   
  - 敵
    - 赤色の四角いオブジェクトが敵です
    - 自機が敵に当たるとゲームオーバーです
  - 敵の弾
    - 黒色のオブジェクトが敵が撃ってくる弾です
    - 当たるとライフが1つ減ります
    - 三種類の動きがあります
      - 直線落下
      - 回転落下
      - 時期に接近
  - Score
    - 終了後のスコア表示画面です
    - Enterを押すとタイトルに戻ります

  - CSVからの敵データ設定について
    - Mainシーンを開いた状態で行います
    - エディタ上のメニュー `Tools/LoadEnemyDataFromCsv`から開きます
    - CSVファイル選択を押すとファイル選択画面が開くのでAssets/Editor/EnemyData.csvを開くとMainシーンの敵情報がCSVからのデータに入れ替わります
   
- こだわったところ
  - 自機の動きに慣性を入れた
  - 敵の動きにいくつかバリエーションを入れた
- 制作期間
  - 9時間
  
-　開発環境
    - Unity 2022.3.20.f1
    - Mac
    - Rider

      
  
