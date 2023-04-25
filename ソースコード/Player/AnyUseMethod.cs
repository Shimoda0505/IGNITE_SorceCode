using UnityEngine;


/// <summary>
/// ‚æ‚­g—p‚·‚éŠÖ”‚Ìƒƒ]ƒbƒhŠÇ—
/// </summary>
public struct AnyUseMethod
{

    #region Mathf

    /// <summary>
    /// MoveToWardsPercentage
    /// </summary>
    /// <param name="startVector"></param>
    /// <param name="targetVector"></param>
    /// <returns>Vector3Œ^‚ÌŠ„‡(Z‚ğ1‚Æ‚µ‚½‚Æ‚«‚ÌX.Y‚ÌŠ„‡(0~1))</returns>
    public Vector3 MoveToWardsPercentage(Vector3 startVector, Vector3 targetVector)
    {
        startVector.x = Mathf.Abs(startVector.x - targetVector.x) / Mathf.Abs(startVector.z - targetVector.z);
        startVector.y = Mathf.Abs(startVector.y - targetVector.y) / Mathf.Abs(startVector.z - targetVector.z);
        startVector.z = 1;

        return startVector;

    }


    /// <summary>
    /// MoveToWards
    /// </summary>
    /// <param name="startVector"></param>
    /// <param name="targetVector"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="percentage"></param>
    /// <returns>Vector3Œ^‚ÌˆÚ“®’l</returns>
    public Vector3 MoveToWardsVector3(Vector3 startVector, Vector3 targetVector, float moveSpeed, Vector3 percentage)
    {
        startVector.x = Mathf.MoveTowards(startVector.x, targetVector.x, moveSpeed * percentage.x);
        startVector.y = Mathf.MoveTowards(startVector.y, targetVector.y, moveSpeed * percentage.y);
        startVector.z = Mathf.MoveTowards(startVector.z, targetVector.z, moveSpeed * percentage.z);

        return startVector;
    }


    /// <summary>
    /// MoveToWardsAngle
    /// </summary>
    /// <param name="startVector"></param>
    /// <param name="targetVector"></param>
    /// <param name="moveSpeed"></param>
    /// <returns>Vector3Œ^‚Ì‰ñ“]’l</returns>
    public Vector3 MoveToWardsAngleVector3(Vector3 startVector, Vector3 targetVector, Vector3 moveSpeed)
    {
        startVector.x = Mathf.MoveTowards(startVector.x, targetVector.x, moveSpeed.x);
        startVector.y = Mathf.MoveTowards(startVector.y, targetVector.y, moveSpeed.y);
        startVector.z = Mathf.MoveTowards(startVector.z, targetVector.z, moveSpeed.z);

        return startVector;
    }


    public Color MoveToWardsColorVector3(Color startVector, Color targetVector, float moveSpeed)
    {
        startVector.r = Mathf.MoveTowards(startVector.r, targetVector.r, moveSpeed);
        startVector.g = Mathf.MoveTowards(startVector.g, targetVector.g, moveSpeed);
        startVector.b = Mathf.MoveTowards(startVector.b, targetVector.b, moveSpeed);

        return startVector;
    }

    /// <summary>
    /// MoveClampVector3
    /// </summary>
    /// <param name="moveVector"></param>
    /// <param name="inputValue"></param>
    /// <param name="speed"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>Vector3Œ^‚Ì“ü—Í‚É‘Î‚·‚é’l‚ªA§ŒÀ‚ğ‚Â‚¯‚Ä•Ô‹p</returns>
    public Vector3 MoveClampVector3(Vector3 moveVector, Vector3 inputValue, Vector3 speed, Vector3 min, Vector3 max)
    {

        moveVector.x = Mathf.Clamp(moveVector.x + inputValue.x * speed.x, min.x, max.x);
        moveVector.y = Mathf.Clamp(moveVector.y + inputValue.y * speed.y, min.y, max.y);
        moveVector.z = Mathf.Clamp(moveVector.z + inputValue.z * speed.z, min.z, max.z);

        return moveVector;
    }


    /// <summary>
    /// LerpClampVector3
    /// </summary>
    /// <param name="startVector"></param>
    /// <param name="endVector"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>Vector3Œ^‚Ì“ü—Í‚É‘Î‚·‚é’l‚ªA§ŒÀ‚ğ‚Â‚¯‚Ä•Ô‹p</returns>
    public Vector3 LerpClampVector3(Vector3 startVector, Vector3 endVector, Vector3 moveSpeed, Vector3 min, Vector3 max)
    {

        startVector.x = Mathf.Clamp(Mathf.Lerp(startVector.x, endVector.x, moveSpeed.x), min.x, max.x);
        startVector.y = Mathf.Clamp(Mathf.Lerp(startVector.y, endVector.y, moveSpeed.y), min.y, max.y);
        startVector.z = Mathf.Clamp(Mathf.Lerp(startVector.z, endVector.z, moveSpeed.z), min.z, max.z);

        return startVector;
    }

    #endregion


    #region Vector
    /// <summary>
    /// InputNomarizeVector2
    /// </summary>
    /// <param name="inputvalue"></param>
    /// <param name="reverse"></param>
    /// <returns>“ü—Í‚ğ³‹K‰»‚µ‚Ä•Ô‹p</returns>
    public Vector2 InputNomarizeVector2(Vector2 inputvalue, float reverse)
    {

        //“ü—Í—Ê
        inputvalue = new Vector2(inputvalue.x, inputvalue.y * reverse);

        //Î‚ß‚Ì“ü—Í‚ğ³‹K‰»
        inputvalue.Normalize();

        return inputvalue;
    }

    #endregion


}
