using UnityEngine;
using System.Collections;

//<summary>
//Basic class for Tree generation logic.
//Subclasses moust override GetCellInRange to implement selecting strategy.
//</summary>
public abstract class TreeMazeGenerator : BasicMazeGenerator {

	//<summary>
	//Class representation of target cell
	//</summary>
	private struct CellToVisit{
		public int Row;
		public int Column;
		public Direction MoveMade;

		public CellToVisit(int row, int column, Direction move){
			Row = row;
			Column = column;
			MoveMade = move;
		}

		public override string ToString ()
		{
			return string.Format ("[MazeCell {0} {1}]",Row,Column);
		}
	}

	System.Collections.Generic.List<CellToVisit> mCellsToVisit = new System.Collections.Generic.List<CellToVisit> ();

	public TreeMazeGenerator(int row, int column):base(row,column){

	}

	private readonly int _MaxNumberOfGlitches = 26;

	private static int _CurrentNumberOfGlitches = 0;
	
	private bool ShouldGlitch(int row, int col)
	{
		
		if ((_CurrentNumberOfGlitches / (col+row+1) < _MaxNumberOfGlitches  ))
		{
			_CurrentNumberOfGlitches++;
			return true;
		}
		else
		{
			return false;
		}

	}
	 
	
	
	public int MAX_ENEMIES = 5;
	
	public override void GenerateMaze ()
	{
		int numEnemies = 0;
		
		Direction[] movesAvailable = new Direction[4];
		int movesAvailableCount = 0;
		mCellsToVisit.Add (new CellToVisit (Random.Range (0, RowCount), Random.Range (0, ColumnCount),Direction.Start));
		
		while (mCellsToVisit.Count > 0) {
			movesAvailableCount = 0;
			CellToVisit ctv = mCellsToVisit[GetCellInRange(mCellsToVisit.Count-1)];

			bool isGlitch = ShouldGlitch(ctv.Row, ctv.Column);
			
			//determine if glitch
			//TODO make this work better A really simplenway of adding glitched walls.
			
			//check move right
			if(ctv.Column+1 < ColumnCount && !GetMazeCell(ctv.Row,ctv.Column+1).IsVisited && !IsCellInList(ctv.Row,ctv.Column+1)){
				movesAvailable[movesAvailableCount] = Direction.Right;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row,ctv.Column).IsVisited && ctv.MoveMade != Direction.Left){
				GetMazeCell(ctv.Row,ctv.Column).WallRight = true;
				if(ctv.Column+1 < ColumnCount){
					GetMazeCell(ctv.Row,ctv.Column+1).WallLeft = true;

					GetMazeCell(ctv.Row, ctv.Column + 1).IsGlitch = isGlitch;
				}
			}
			//check move forward
			if(ctv.Row+1 < RowCount && !GetMazeCell(ctv.Row+1,ctv.Column).IsVisited && !IsCellInList(ctv.Row+1,ctv.Column)){
				movesAvailable[movesAvailableCount] = Direction.Front;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row,ctv.Column).IsVisited && ctv.MoveMade != Direction.Back){
				GetMazeCell(ctv.Row,ctv.Column).WallFront = true;
				if(ctv.Row+1 < RowCount){
					GetMazeCell(ctv.Row+1,ctv.Column).WallBack = true;
					
					GetMazeCell(ctv.Row+1, ctv.Column).IsGlitch =  isGlitch;
				}
			}
			//check move left
			if(ctv.Column > 0 && ctv.Column-1 >= 0 && !GetMazeCell(ctv.Row,ctv.Column-1).IsVisited && !IsCellInList(ctv.Row,ctv.Column-1)){
				movesAvailable[movesAvailableCount] = Direction.Left;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row,ctv.Column).IsVisited && ctv.MoveMade != Direction.Right){
				GetMazeCell(ctv.Row,ctv.Column).WallLeft = true;
				if(ctv.Column > 0 && ctv.Column-1 >= 0){
					GetMazeCell(ctv.Row,ctv.Column-1).WallRight = true;
					
					GetMazeCell(ctv.Row, ctv.Column - 1).IsGlitch =  isGlitch;
				}
			}
			//check move backward
			if(ctv.Row > 0 && ctv.Row-1 >= 0 && !GetMazeCell(ctv.Row-1,ctv.Column).IsVisited && !IsCellInList(ctv.Row-1,ctv.Column)){
				movesAvailable[movesAvailableCount] = Direction.Back;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row,ctv.Column).IsVisited && ctv.MoveMade != Direction.Front){
				GetMazeCell(ctv.Row,ctv.Column).WallBack = true;
				if(ctv.Row > 0 && ctv.Row-1 >= 0){
					GetMazeCell(ctv.Row-1,ctv.Column).WallFront = true;
					
					GetMazeCell(ctv.Row-1, ctv.Column).IsGlitch =  isGlitch;
				}
			}

			if(!GetMazeCell(ctv.Row,ctv.Column).IsVisited && movesAvailableCount == 0){
				GetMazeCell(ctv.Row,ctv.Column).IsGoal = true;
			}

			GetMazeCell(ctv.Row,ctv.Column).IsVisited = true;
			
			if(movesAvailableCount > 0){
				switch(movesAvailable[Random.Range(0,movesAvailableCount)]){
				case Direction.Start:
					break;
				case Direction.Right:
					mCellsToVisit.Add(new CellToVisit(ctv.Row,ctv.Column+1,Direction.Right));
					break;
				case Direction.Front:
					mCellsToVisit.Add(new CellToVisit(ctv.Row+1,ctv.Column,Direction.Front));
					break;
				case Direction.Left:
					mCellsToVisit.Add(new CellToVisit(ctv.Row,ctv.Column-1,Direction.Left));
					break;
				case Direction.Back:
					mCellsToVisit.Add(new CellToVisit(ctv.Row-1,ctv.Column,Direction.Back));
					break;
				}
			}else{
				mCellsToVisit.Remove(ctv);
			}
		}

		for (; numEnemies < MAX_ENEMIES; numEnemies++)
		{
			int x = Random.Range(0,RowCount);
			int y = Random.Range(1, ColumnCount);
			GetMazeCell(x, y).IsSpawn = true;
		}
	}

	private bool IsCellInList(int row, int column){
		return mCellsToVisit.FindIndex ((other) => other.Row == row && other.Column == column) >= 0;
	}

	//<summary>
	//Abstract method for cell selection strategy
	//</summary>
	protected abstract int GetCellInRange(int max);
}
