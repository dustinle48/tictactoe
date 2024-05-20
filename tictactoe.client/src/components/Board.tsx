import { calculateWinner } from "../utils/calculateWinner";
import Square from "./Square";

type BoardProps = {
  currentGameId: number | null;
  xIsNext: boolean;
  squares: string[];
  onPlay: (nextSquares: string[]) => void;
  onNewGame: () => void;
};

function Board(props: BoardProps) {
  const { currentGameId, xIsNext, squares, onPlay, onNewGame } = props;

  function handleClick(i: number) {
    if (calculateWinner(squares) || squares[i]) {
      return;
    }
    const nextSquares = squares.slice();
    if (xIsNext) {
      nextSquares[i] = "X";
    } else {
      nextSquares[i] = "O";
    }
    onPlay(nextSquares);
  }

  const winner = calculateWinner(squares);
  let status;
  if (winner) {
    status = "Winner: " + winner;
  } else {
    status = "Next player: " + (xIsNext ? "X" : "O");
  }

  return (
    <>
      {currentGameId ? (
        <>
          <div>Playing game #{currentGameId}</div>
          <div className="status">{status}</div>
          {[...Array(3).keys()].map((row: number) => (
            <div key={row} className="board-row">
              {[...Array(3).keys()].map((col: number) => (
                <Square
                  key={col}
                  value={squares[3 * row + col]}
                  onSquareClick={() => handleClick(3 * row + col)}
                />
              ))}
            </div>
          ))}
        </>
      ) : (
        <>
          <button onClick={onNewGame}>New game</button>
        </>
      )}
    </>
  );
}

export default Board;
