using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Travel : MonoBehaviour {

	private static int travelButton;
	private static float travelSpeed;
	private static int backButton;
	private static int backButtonCounter;

	private static Transform hallwayTransform;
	private static Transform hallway1Transform;
	private static Transform hallway2Transform;
	private static Transform hallway3Transform;
	private static Transform startingPoint;
	private static Transform endWall;

	private static bool isUsed = false;

	private static Vector3 navigationTransformPosition = new Vector3(0,0,0);	
	private static Vector3 mainCameraTransformPosition = new Vector3(0,0,0);

	private static List<string> listEntry = new List<string>();	
	private static List<string> gameobjectList = new List<string>();

	private static bool first = false;

	//private string rootFolderPath = "C:\\Users\\Snow_Leopard\\Downloads\\crude\\1";

	private string rootFolderPath = "/sdcard";
	// Use this for initialization
	void Start () {

		startingPoint = GameObject.Find("Starting Point").transform;
		hallwayTransform = GameObject.Find("Hallway0").transform;
		hallway1Transform = GameObject.Find("Hallway1").transform;
		hallway2Transform = GameObject.Find("Hallway2").transform;
		hallway3Transform = GameObject.Find ("Hallway3").transform;
		endWall = GameObject.Find("End_wall").transform;

		travelButton = 0;
		backButton = 1;
		travelSpeed = 1.0f;

		onCreateScene(rootFolderPath);

		setInitialPosition();

	}

	// Update is called once per frame
	void Update () {

		Quaternion gazeDirection = GameObject.Find ("Main Camera").transform.localRotation;

		RayCastSpecimen raycastspecimen = new RayCastSpecimen();

		raycastspecimen = onRaycasting();

		Debug.Log (raycastspecimen.foldertext);

		Debug.Log(GameObject.Find("Ray").transform.position);

		// Back Button Event
		if(Input.GetMouseButton(backButton)){

			if(listEntry.Count != 1
			   && backButtonCounter < 1){

				listEntry.RemoveAt(listEntry.Count - 1);

				string filepath = listEntry[listEntry.Count - 1];

				listEntry.RemoveAt(listEntry.Count - 1);

				onDestroyScene();

				onCreateScene(filepath);

				getInitialPosition();

				backButtonCounter++;

			}
		} else {

			if(Input.GetMouseButton(travelButton)) {

				// Initializing the back counter variable to launch at the right click event or back button event
				if(backButtonCounter != 0){
					
					backButtonCounter = 0;
					
				}

				if(raycastspecimen.foldertext != "null"){

					if(isUsed){

						onDestroyScene();
						
						onCreateScene(listEntry[listEntry.Count - 1] + "//" + raycastspecimen.foldertext);

						//onCreateScene(listEntry[listEntry.Count - 1] + "\\" + raycastspecimen.foldertext);

						getInitialPosition();

					} else{

						isUsed = true;

					}


				} else {

					if(Vector3.Distance(GameObject.Find("Ray").transform.position, GameObject.Find("Main Camera").transform.position) > 1){

						Vector3 travelVector = gazeDirection * Vector3.forward * travelSpeed;
						travelVector.y = 0;
						
						GameObject.Find ("Travel").transform.Translate(travelVector);

					}

				}
				

			}

		}

	}

	void onCreateScene(string filepath){

		listEntry.Add (filepath);

		int roomNumber = 0;

		int count = 0;

		int wall_counter = 0;

		string wall_to_end = "endwall";

		string[] directories = Directory.GetDirectories(filepath);

		string[] list = Directory.GetFiles(filepath);

		int fileFolderLength = list.Length / 18;

		int fileRemainder = list.Length % 18;

		bool isFiles = false;

		bool isDirectory = false;

		bool isSingleFolderActive = false;

		bool isDoubleFolderActive = false;

		int i = 1;

		int j = 1;

		if(list.Length != 0){

			if (fileFolderLength > 0){
				
				isFiles = true;
				
				for(j = 1; j <= fileFolderLength; j++){
					
					Object folder;
					
					folder = Instantiate(hallwayTransform, new Vector3(-0.018f,0,
					                                                   j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
					                     + startingPoint.transform.position, Quaternion.identity);
					
					
					folder.name = "Hallway0" + count;
					
					gameobjectList.Add(folder.name);
					
					for (int k = 0; k < 9; k++){
						
						TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(4).transform.GetChild(k).gameObject.GetComponent<TextMesh>();
						
						text1.text = Path.GetFileName(list[roomNumber]);
						
						text1.color = Color.black;
						
						roomNumber++;
						
					}
					
					for (int k = 0; k < 9; k++){
						
						TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(3).transform.GetChild(9 + k).gameObject.GetComponent<TextMesh>();
						
						text1.text = Path.GetFileName(list[roomNumber]);
						
						text1.color = Color.black;
						
						roomNumber++;
						
					}
					
					count++;
					
				}
				
				if(fileRemainder > 0){
					
					if(fileRemainder == 9){
						
						Object folder = Instantiate(hallway3Transform, new Vector3(-0.018f,0,
						                                                           j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
						                            + startingPoint.transform.position, Quaternion.identity);
						
						folder.name = "Hallway3" + count;
						
						gameobjectList.Add(folder.name);
						
						for(int k = 0; k < 9; k++){
							
							TextMesh text1 = GameObject.Find("Hallway3" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
							
							text1.text = Path.GetFileName(list[roomNumber]);
							
							text1.color = Color.black;
							
							roomNumber++;
							
						}
						
						
					} else {
						
						if(fileRemainder < 9){
							
							Object folder = Instantiate(hallway3Transform, new Vector3(-0.018f,0,
							                                                           j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
							                            + startingPoint.transform.position, Quaternion.identity);
							
							folder.name = "Hallway3" + count;
							
							gameobjectList.Add(folder.name);
							
							for(int k = 0; k < 9; k++){
								
								if( k < fileRemainder){
									
									TextMesh text1 = GameObject.Find("Hallway3" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
									
									text1.text = Path.GetFileName(list[roomNumber]);
									
									text1.color = Color.black;
									
									roomNumber++;
									
								} else {
									
									TextMesh text1 = GameObject.Find("Hallway3" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
									
									text1.text = "";
									
								}
								
							}
							
						} else {
							
							Object folder = Instantiate(hallwayTransform, new Vector3(-0.018f,0,
							                                                          j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
							                            + startingPoint.transform.position, Quaternion.identity);
							
							folder.name = "Hallway0" + count;
							
							gameobjectList.Add(folder.name);
							
							for(int k = 0; k < 9; k++){
								
								TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
								
								text1.text = Path.GetFileName(list[roomNumber]);
								
								text1.color = Color.black;
								
								roomNumber++;
								
								fileRemainder--;
							}
							
							for(int k = 0; k < 9; k++){
								
								if( k < fileRemainder){
									
									TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(4).transform.GetChild(k).gameObject.GetComponent<TextMesh>();
									
									text1.text = Path.GetFileName(list[roomNumber]);
									
									text1.color = Color.black;
									
									roomNumber++;
									
								} else {
									
									TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(4).transform.GetChild(k).gameObject.GetComponent<TextMesh>();
									
									text1.text = "";
									
								}
								
							}
							
						}
						
						
					}
					
				}
				
			} else {
				
				if(fileRemainder > 0){
					
					isFiles = true;
					
					if(fileRemainder == 9){
						
						Object folder = Instantiate(hallway3Transform, new Vector3(-0.018f,0,
						                                                           j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
						                            + startingPoint.transform.position, Quaternion.identity);
						
						folder.name = "Hallway3" + count;
						
						gameobjectList.Add(folder.name);
						
						for(int k = 0; k < 9; k++){
							
							TextMesh text1 = GameObject.Find("Hallway3" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
							
							text1.text = Path.GetFileName(list[roomNumber]);
							
							text1.color = Color.black;
							
							roomNumber++;
							
						}
						
						
					} else {
						
						if(fileRemainder < 9){
							
							Object folder = Instantiate(hallway3Transform, new Vector3(-0.018f,0,
							                                                           j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
							                            + startingPoint.transform.position, Quaternion.identity);
							
							folder.name = "Hallway3" + count;
							
							gameobjectList.Add(folder.name);
							
							for(int k = 0; k < 9; k++){
								
								if( k < fileRemainder){
									
									TextMesh text1 = GameObject.Find("Hallway3" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
									
									text1.text = Path.GetFileName(list[roomNumber]);
									
									text1.color = Color.black;
									
									roomNumber++;
									
								} else {
									
									TextMesh text1 = GameObject.Find("Hallway3" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
									
									text1.text = "";
									
								}
								
							}
							
						} else {
							
							Object folder = Instantiate(hallwayTransform, new Vector3(-0.018f,0,
							                                                          j * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
							                            + startingPoint.transform.position, Quaternion.identity);
							
							folder.name = "Hallway0" + count;
							
							gameobjectList.Add(folder.name);
							
							for(int k = 0; k < 9; k++){
								
								TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(3).transform.GetChild(k + 9).gameObject.GetComponent<TextMesh>();
								
								text1.text = Path.GetFileName(list[roomNumber]);
								
								text1.color = Color.black;
								
								roomNumber++;
								
								fileRemainder--;
								
							}
							
							for(int k = 0; k < 9; k++){
								
								if( k < fileRemainder){
									
									TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(4).transform.GetChild(k).gameObject.GetComponent<TextMesh>();
									
									text1.text = Path.GetFileName(list[roomNumber]);
									
									text1.color = Color.black;
									
									roomNumber++;
									
								} else {
									
									TextMesh text1 = GameObject.Find("Hallway0" + count).transform.GetChild(4).transform.GetChild(k).gameObject.GetComponent<TextMesh>();
									
									text1.text = "";
									
								}
								
							}
							
						}
						
						
					}
					
				}
				
			}
		}

		Object newfolder;

		if(isFiles){

			newfolder = Instantiate(endWall, new Vector3(0,1.5f,(j + 1) * 1.50f * GameObject.Find("Starting Point").transform.localScale.z) 
			                        + startingPoint.transform.position, Quaternion.identity);

		} else {

			newfolder = Instantiate(endWall, new Vector3(0,1.5f,j * 1f * GameObject.Find("Starting Point").transform.localScale.z) 
			                        + startingPoint.transform.position, Quaternion.identity);

		}

		newfolder.name = wall_to_end + wall_counter;

		wall_counter++;

		gameobjectList.Add(newfolder.name);

		roomNumber = 0;

		if(directories.Length != 0){

			isDirectory = true;

		}

		for(i = 1; i <= directories.Length / 2; i++){

			isDoubleFolderActive = true;

			Object folder;

			folder = Instantiate(hallway2Transform, new Vector3(-0.018f,0,
				                                                  -i * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
										                            + startingPoint.transform.position, Quaternion.identity);

			folder.name = "Hallway2" + count;

			gameobjectList.Add(folder.name);

			TextMesh text1 = GameObject.Find("Hallway2" + count).transform.GetChild(0).transform.GetChild(3).gameObject.GetComponent<TextMesh>();
			TextMesh text2 = GameObject.Find("Hallway2" + count).transform.GetChild(4).transform.GetChild(3).gameObject.GetComponent<TextMesh>();

			text1.text = Path.GetFileName(directories[roomNumber]);
			text2.text = Path.GetFileName(directories[roomNumber + 1]);

			text1.color = Color.black;
			text2.color = Color.black;

			roomNumber += 2;

			count++;

		}

		if(directories.Length % 2 != 0){

			isSingleFolderActive = true;

			Object folder = Instantiate(hallway1Transform, new Vector3(0.026f,0,
			                                                          -i * 2 * GameObject.Find("Starting Point").transform.localScale.z) 
			                            + startingPoint.transform.position, Quaternion.identity);

			folder.name = "Hallway1" + count;

			gameobjectList.Add(folder.name);
			
			TextMesh text1 = GameObject.Find("Hallway1" + count).transform.GetChild(0).transform.GetChild(3).gameObject.GetComponent<TextMesh>();
			
			text1.text = Path.GetFileName(directories[directories.Length - 1]);

			text1.color = Color.black;

			count++;

		}

		Object wallObject;

		if(isDirectory){

			if(isDoubleFolderActive){
				if(isSingleFolderActive){
					wallObject = Instantiate(endWall, new Vector3(0,1.5f,-(i + 1) * 1.94f * GameObject.Find("Starting Point").transform.localScale.z) 
					                         + startingPoint.transform.position, Quaternion.identity);
				} else {
					wallObject = Instantiate(endWall, new Vector3(0,1.5f,-(i + 1) * 1.82f * GameObject.Find("Starting Point").transform.localScale.z) 
					                         + startingPoint.transform.position, Quaternion.identity);
				}
			} else {
				if(isSingleFolderActive){
					wallObject = Instantiate(endWall, new Vector3(0,1.5f,-(i + 1) * 1.47f * GameObject.Find("Starting Point").transform.localScale.z) 
					                         + startingPoint.transform.position, Quaternion.identity);
				} else {
					wallObject = Instantiate(endWall, new Vector3(0,1.5f,-i * 1f * GameObject.Find("Starting Point").transform.localScale.z) 
					                         + startingPoint.transform.position, Quaternion.identity);
				}
			}


		} else {

			wallObject = Instantiate(endWall, new Vector3(0,1.5f,-i * 1f * GameObject.Find("Starting Point").transform.localScale.z) 
			                         + startingPoint.transform.position, Quaternion.identity);

		}

		wallObject.name = wall_to_end + wall_counter;
		
		wall_counter++;

		gameobjectList.Add(wallObject.name);

	}

	void onDestroyScene(){
		foreach (string key in gameobjectList){
			Destroy(GameObject.Find(key));
		}
		gameobjectList = new List<string>();
	}

	// Get Initial Position of Navigation and its child
	private static void getInitialPosition(){
				
		GameObject.Find ("Travel").transform.position = navigationTransformPosition;
		
		GameObject.Find("Main Camera").transform.position = mainCameraTransformPosition;
		
	}

	private static void setInitialPosition(){
		
		navigationTransformPosition = GameObject.Find ("Travel").transform.position;
		
		mainCameraTransformPosition = GameObject.Find("Main Camera").transform.position;
		
	}

	// Fetch Raycasted Object Name. In case of exception the object is null.
	private static RayCastSpecimen onRaycasting(){
		
		// Get the head position and gaze direction from the Main Camera object
		Vector3 headPosition = GameObject.Find("Main Camera").transform.position;
		
		Vector3 gazeDirection = GameObject.Find("Main Camera").transform.forward;	

		RayCastSpecimen raycastspecimen = new RayCastSpecimen();
		
		string gameObjectName = "";

		string parentName = "";

		string folder_door_right = "RightDoor";

		string folder_door_left = "LeftDoor";
		
		RaycastHit hitInfo;

		try{

			Physics.Raycast(headPosition, gazeDirection, out hitInfo);

			GameObject.Find("Ray").transform.position = hitInfo.point;

			parentName = hitInfo.collider.gameObject.transform.parent.name;

			if(parentName == folder_door_left || parentName == folder_door_right){
				raycastspecimen.foldertext = hitInfo.collider.gameObject.transform.parent.transform.GetChild(3).gameObject.GetComponent<TextMesh>().text;
			} else {
				raycastspecimen.foldertext = "null";
			}
			
		} catch {
			
			raycastspecimen.foldertext = "null";
			
		}
		
		return raycastspecimen;
		
	}

}
