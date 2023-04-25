using UnityEngine;



/// <summary>
/// プレイヤーのカラーをウルト時に青に変更
/// プレイヤーの透明度をカーソルが重なっているかにより変更
/// </summary>
public class PlayerTransparent : MonoBehaviour
{

    #region スクリプト
    [SerializeField]
    GameSystem _gameSystem;

    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    PlayerStatus _playerStatus;
    #endregion

    #region カーソル
    [Header("カーソル関連")]
    [SerializeField, Tooltip("Cursor")]
    private Transform _cursorPos;

    [SerializeField]
    private float _distance;

    //Ui座標の最大値
    private Vector2 _maxRectTr = new Vector2(800, 450);

    //カメラ座標の値を(-0.5~0.5)から(0~1)に変換
    private const float _valueChange = 0.5f;

    [SerializeField]
    private Camera _mainCamera;
    #endregion

    #region マテリアル
    [Header("マテリアル関連")]
    [SerializeField, Tooltip("メッシュ")]
    private SkinnedMeshRenderer _mesh;

    [SerializeField, Tooltip("透過マテリアル")]
    private Material[] _transMaterials;

    [SerializeField, Tooltip("非透過マテリアル")]
    private Material[] _opaMaterials;

    [SerializeField, Tooltip("透過マテリアル(ULT)")]
    private Material[] _transMaterialsUlt;

    [SerializeField, Tooltip("非透過マテリアル(ULT)")]
    private Material[] _opaMaterialsUlt;

    //ウルト中かどうか
    private bool _isUlt = false;
    #endregion

    #region ディゾルブ
    [Header("ディゾルブ関連")]
    [SerializeField, Tooltip("差し替え速度")]
    private float _changeDisSpeed;

    //ディゾルブID
    private int _dis = 0;

    //ディゾルブの現在値
    private float _disMove = 0;

    //差し替え
    private bool _isDis = false;
    #endregion

    #region ブーリアン
    [Header("ブーリアン関連")]
    [SerializeField, Tooltip("ブーリアン速度")]
    private float _changeBospeed;

    //ブーリアンID
    private int _bo;

    //リムパワーID
    private int _rim;

    //ブーリアンの現在値
    [SerializeField]
    private float _rimMove;

    //ブーリアンの最大最小
    private Vector2 _rimClamp = new Vector2(1, 0.4f);
    #endregion

    #region エフェクト
    [Header("エフェクト")]
    [SerializeField, Tooltip("赤炎")]
    private GameObject _redFire;

    [SerializeField, Tooltip("青炎")]
    private GameObject _blueFire;
    #endregion


    Motion _motion = Motion.OPAQUA;
    enum Motion
    {
        CHANGE,
        TRANSPARENT,
        OPAQUA
    }



    //処理部---------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //エフェクトのActiveをfalse
        _redFire.SetActive(false);
        _blueFire.SetActive(false);

        //ブーリアンID取得
        _bo = Shader.PropertyToID("_Boolean");

        //リムパワーID取得
        _rim = Shader.PropertyToID("_RimPower");

        //リム値の初期値
        _rimMove = _rimClamp.x;

        //リムの変更
        //ChangeFloat(_transMaterials, _rim, _rimMove);
        ChangeFloat(_opaMaterials, _rim, _rimMove);

        //ブーリアンの変更
        ChangeFloat(_transMaterials, _bo, 0);
        ChangeFloat(_opaMaterials, _bo, 0);


        //Opaquaに初期設定
        ChangeMaterial(_opaMaterials);


    }

    private void FixedUpdate()
    {

        //イベント中 / ウルトマテリアル差し替え / 非透過に遷移
        if (_gameSystem._isEvent)
        {
            if (_playerController.IsUlt()) { ChangeMaterial(_opaMaterialsUlt); _redFire.SetActive(false); _blueFire.SetActive(true); }
            else { ChangeMaterial(_opaMaterials); }

            _motion = Motion.OPAQUA;

            return;
        }


        //ウルト使用可能時のプレイヤー光沢
        ChangeGloss();

        //レティクルとプレイヤーの位置を計算/enumの遷移
        ReticleDistance();

        //炎のActive
        EffectActive();

        //透過
        switch (_motion)
        {

            //透過中
            case Motion.TRANSPARENT:

                //非透過
                if (!_playerController.IsUlt()) { ChangeMaterial(_transMaterials); }
                //非透過Ult中
                else if (_playerController.IsUlt()) { ChangeMaterial(_transMaterialsUlt); }

                break;


            //非透過中
            case Motion.OPAQUA:

                //非透過
                if (!_playerController.IsUlt()) { ChangeMaterial(_opaMaterials); }
                //非透過Ult中
                else if (_playerController.IsUlt()) { ChangeMaterial(_opaMaterialsUlt); }

                break;

        }

    }



    //メソッド部------------------------------------------------------------------------------------------------------

    //レティクル
    /// <summary>
    /// レティクルとプレイヤーの位置を計算/enumの遷移
    /// </summary>
    private void ReticleDistance()
    {

        //プレイヤーの座標を、カメラView座標(0~1,0~1)に変換
        Vector2 thisPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);

        //ポインターのカメラView座標(0~1,0~1)
        //Vector(0~800 , 0~460)を(0~1 , 0~1)に変換
        Vector2 uiPos = new Vector2(_cursorPos.localPosition.x / _maxRectTr.x + _valueChange,
                                    _cursorPos.localPosition.y / _maxRectTr.y + _valueChange);

        //ポインターとエネミーの距離を計測
        float distans = Vector2.Distance(uiPos, thisPos);

        //透過に遷移
        if (-_distance <= distans && distans <= _distance) { _motion = Motion.TRANSPARENT; }
        //非透過に遷移
        else if (-_distance > distans || distans > _distance) { _motion = Motion.OPAQUA; }

    }

    //エフェクト
    /// <summary>
    /// 炎のActive
    /// </summary>
    private void EffectActive()
    {

        if (_playerController.IsUlt() && !_blueFire.activeSelf)
        {
            //赤炎のStop
            _redFire.SetActive(false);

            //青炎のActive
            _blueFire.SetActive(true);
        }
        else if (!_playerController.IsUlt() && _blueFire.activeSelf)
        {

            //青炎のStop
            _blueFire.SetActive(false);
        }

    }

    //マテリアル
    /// <summary>
    /// マテリアル差し替え
    /// </summary>
    private void ChangeMaterial(Material[] material)
    {

        Material[] tmp = _mesh.materials;
        tmp[0] = material[0];
        tmp[1] = material[1];
        _mesh.materials = tmp;

    }

    /// <summary>
    /// ウルト使用可能時のプレイヤー光沢
    /// </summary>
    private void ChangeGloss()
    {

        if (_playerStatus.IsSkillMax())
        {

            //同値なら処理しない
            if (_rimMove <= _rimClamp.y) { return; }

            //赤炎のActive
            _redFire.SetActive(true);

            //ブーリアンの表示
            _rimMove = Mathf.MoveTowards(_rimMove, _rimClamp.y, Time.deltaTime * _changeBospeed);

            //ブーリアンの変更
            ChangeFloat(_transMaterials, _bo, 1);
            ChangeFloat(_opaMaterials, _bo, 1);

            //リムの変更
            //ChangeFloat(_transMaterials, _rim, _rimMove);
            ChangeFloat(_opaMaterials, _rim, _rimMove);

        }
        else
        {

            //同値なら処理しない
            if (_rimMove >= _rimClamp.x) { return; }

            //赤炎のStop
            _redFire.SetActive(false);

            //ブーリアンの非表示
            _rimMove = Mathf.MoveTowards(_rimMove, _rimClamp.x, Time.deltaTime * _changeBospeed);

            //ブーリアンフラグの変更
            ChangeFloat(_transMaterials, _bo, 0);
            ChangeFloat(_opaMaterials, _bo, 0);

            //リムの変更
            //ChangeFloat(_transMaterials, _rim, _rimMove);
            ChangeFloat(_opaMaterials, _rim, _rimMove);

        }

    }

    /// <summary>
    /// マテリアル値の変更(Dissolve / Rim)
    /// </summary>
    private void ChangeFloat(Material[] materials, int id, float speed)
    {
        //マテリアルのディゾルブ
        materials[0].SetFloat(id, speed);
        materials[1].SetFloat(id, speed);

    }

    /// <summary>
    /// ディゾルブ変更
    /// </summary>
    private void ChangeDissolve(Material[] plusMaterial, Material[] minusMaterial)
    {

        //消失
        if (!_isDis)
        {

            //ディゾルブ加算
            _disMove += Time.deltaTime * _changeDisSpeed;

            if (_disMove >= 1)
            {

                _disMove = 1;

                //マテリアルの変更
                ChangeMaterial(minusMaterial);

                _isDis = true;

            }

            //マテリアルのディゾルブ
            ChangeFloat(plusMaterial, _dis, _disMove);

        }
        //出現
        else if (_isDis)
        {

            //ディゾルブ減算
            _disMove -= Time.deltaTime * _changeDisSpeed;

            if (_disMove <= 0)
            {

                _disMove = 0;

                //ウルト中の変更
                if (_isUlt) { _isUlt = false; }
                else if (!_isUlt) { _isUlt = true; }

                _isDis = false;

                _motion = Motion.OPAQUA;

            }

            //マテリアルのディゾルブ
            ChangeFloat(minusMaterial, _dis, _disMove);

        }

    }

}
