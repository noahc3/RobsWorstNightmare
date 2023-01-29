using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 2023;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject GlitchWall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;

	private BasicMazeGenerator mMazeGenerator = null;

	private GameObject ActiveEspresso;
	private List<Vector3> GoalPosList = new List<Vector3>();

	void Start () {
		if (!FullRandom) {
			Random.seed = RandomSeed;
		}
		switch (Algorithm) {
			case MazeGenerationAlgorithm.PureRecursive:
				mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveTree:
				mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RandomTree:
				mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.OldestTree:
				mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveDivision:
				mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
				break;
		}
		mMazeGenerator.GenerateMaze ();
		
		for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
				tmp.transform.parent = transform;
				
				
				if(cell.WallRight){

					if (cell.IsGlitch)
					{
						tmp = Instantiate(GlitchWall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position,
							Quaternion.Euler(0, 90, 0)) as GameObject; // right
					}
					else
					{
						tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position,
							Quaternion.Euler(0, 90, 0)) as GameObject; // right
					}

					tmp.transform.parent = transform;
				}
				if(cell.WallFront){

					if (cell.IsGlitch)
					{
						tmp = Instantiate(GlitchWall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position,
							Quaternion.Euler(0, 0, 0)) as GameObject; // front
					}
					else
					{
						tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position,
							Quaternion.Euler(0, 0, 0)) as GameObject; // front
					}

					tmp.transform.parent = transform;
				}
				if(cell.WallLeft){
					if (cell.IsGlitch)
					{
						tmp = Instantiate(GlitchWall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position,
							Quaternion.Euler(0, 270, 0)) as GameObject; // left
					}
					else
					{
						tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position,
							Quaternion.Euler(0, 270, 0)) as GameObject; // left
					}
					tmp.transform.parent = transform;
				}
				
				if(cell.WallBack){
					if (cell.IsGlitch)
					{
						tmp = Instantiate(GlitchWall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position,
							Quaternion.Euler(0, 180, 0)) as GameObject; // back
					}
					else
					{
						tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position,
							Quaternion.Euler(0, 180, 0)) as GameObject; // back
					}
					tmp.transform.parent = transform;
				}
				
				if(cell.IsGoal && GoalPrefab != null){
					GoalPosList.Add(new Vector3(x, 0.5f, z));
				}
			}
		}
		
		if(Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
	}

	void Update()
    {
		if (ActiveEspresso == null)
        {
			int rng = Random.Range(0, GoalPosList.Count);
			Vector3 nextPos = GoalPosList[rng];
			GoalPosList.RemoveAt(rng);

			Debug.Log($"Creating new espresso at ({nextPos.x}, {nextPos.z})");
			ActiveEspresso = Instantiate(GoalPrefab, nextPos, Quaternion.Euler(0, 0, 0)) as GameObject;
			ActiveEspresso.transform.parent = transform;
		}
    }
}
