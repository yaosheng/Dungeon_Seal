using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{

    public Camera mainCamera
    {
        get {
            return GameObject.FindObjectOfType<Camera>( ) as Camera;
        }
    }

    private float offerX = 2.5f;
    private float offerZ = -8.5f;
    private float speed = 0.3f;
    private Vector3 offerVector;

    void Start( )
    {
        offerVector = new Vector3(offerX, mainCamera.transform.position.y, offerZ);
    }

    public void SetCameraPosition( Floor floor )
    {
        mainCamera.transform.position = new Vector3(floor.Position( ).x + offerX, mainCamera.transform.position.y, floor.Position( ).z + offerZ);
    }

    public void MoveX( Floor floor )
    {
        iTween.MoveTo(mainCamera.gameObject, iTween.Hash("x", floor.Position( ).x + offerX, "time", speed, "easetype", iTween.EaseType.linear));
        //iTween.MoveTo(mainCamera.gameObject, iTween.Hash("position", floor.Position( ) + offerVector, "time", speed, "easetype", iTween.EaseType.linear));
    }

    public void MoveZ( Floor floor )
    {
        iTween.MoveTo(mainCamera.gameObject, iTween.Hash("z", floor.Position( ).z + offerZ, "time", speed, "easetype", iTween.EaseType.linear));
        //iTween.MoveTo(mainCamera.gameObject, iTween.Hash("position", floor.Position( ) + offerVector, "time", speed, "easetype", iTween.EaseType.linear));
    }

    public void MovePosition(Floor floor )
    {
        iTween.MoveTo(mainCamera.gameObject, iTween.Hash("position", floor.Position( ) + offerVector, "time", speed, "easetype", iTween.EaseType.linear));
    }
}
