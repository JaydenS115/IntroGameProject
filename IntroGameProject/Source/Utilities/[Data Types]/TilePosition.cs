using Godot;

// Tile Position within Hexagonal Grid
public class TilePosition
{
    // X and Y coordinates within Hex Grid
    public int x;
    public int y;


    // Constructors  -  Vector2  and  int, int
    public TilePosition(Vector2 vector) {
        vector = vector.Round(); // round to nearest int
        x = (int)vector.x;
        y = (int)vector.y;
    }
    public TilePosition(int X, int Y){
        x = X;
        y = Y;
    }


    // Conversion (explicitly) between TilePosition and Vector2
    public static explicit operator TilePosition(Vector2 vector) {
        return (new TilePosition(vector));
    }
    public static explicit operator Vector2(TilePosition tilePos) {
        return ( new Vector2( (float)tilePos.x ,  (float)tilePos.y ) );
    }

    
	// return the tile position of a hex in the given direction,
	// relative to the current TilePos stored
	public TilePosition TilePositionOf(HexDirection direction) {
		
		TilePosition targetTilePos = new TilePosition(x, y);

		switch (direction) {
			
			case HexDirection.UpLeft:
				if(Mathf.Abs(x) % 2 == 0) --targetTilePos.y;
				--targetTilePos.x;
				break;

			case HexDirection.Up:
				--targetTilePos.y;
				break;

			case HexDirection.UpRight:
				if(Mathf.Abs(x) % 2 == 0) --targetTilePos.y;
				++targetTilePos.x;
				break;

			case HexDirection.DownLeft:
				if(Mathf.Abs(x) % 2 == 1) ++targetTilePos.y;
				--targetTilePos.x;
				break;

			case HexDirection.Down:
				++targetTilePos.y;
				break;

			case HexDirection.DownRight:
				if(Mathf.Abs(x) % 2 == 1) ++targetTilePos.y;
				++targetTilePos.x;
				break;

			default:	// case HexDirection.None:
				break; 	// just returns current TilePos
		}

		return targetTilePos;
	}


}
