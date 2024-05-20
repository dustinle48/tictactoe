import { useEffect } from "react";
import { calculateWinner } from "../utils/calculateWinner";
import Square from "./Square";

type BoardProps = {
  currentGameId: number | null;
  xPlayerId: number | null;
  oPlayerId: number | null;
  xIsNext: boolean;
  squares: string[];
  onPlay: (nextSquares: string[]) => void;
};

function Board(props: BoardProps) {
  const { currentGameId, xPlayerId, oPlayerId, xIsNext, squares, onPlay } =
    props;

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

  // useEffect(() => {
  //   async function saveGame() {
  //     const text = "Do you want to save the game?";

  //     if (confirm(text) == true) {
  //       const data = {
  //         xPlayerId,
  //         oPlayerId,
  //         winner,
  //       };

  //       try {
  //         await fetch(`https://localhost:7020/api/Game`, {
  //           method: "POST",
  //           credentials: "include",
  //           headers: {
  //             "Content-Type": "application/json; charset=utf-8",
  //           },
  //           body: JSON.stringify(data),
  //         });
  //       } catch (e) {
  //         console.error(e);
  //       }
  //     }
  //   }

  //   if (winner === "X" || winner === "O" || winner === "Draw") {
  //     saveGame();
  //   }
  // }, [oPlayerId, xPlayerId, winner]);

  return (
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
  );
}

export default Board;
