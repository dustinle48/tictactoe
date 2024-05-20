import { useEffect, useState } from "react";
import "./App.css";
import Board from "./components/Board";
import LoginBoard from "./components/LoginBoard";
import { User } from "./interfaces/IUser";
import { IGame } from "./interfaces/IGame";
import { calculateWinner } from "./utils/calculateWinner";
import { getCookie } from "./utils/cookieUtils";
import GameList from "./components/GameList";

function App() {
  const [xPlayer, setXPlayer] = useState<User | null>(null);
  const [oPlayer, setOPlayer] = useState<User | null>(null);
  const [games, setGames] = useState<IGame[]>([]);
  const [currentGame, setCurrentGame] = useState<IGame | null>(null);

  const [history, setHistory] = useState([Array(9).fill(null)]);
  const [currentMove, setCurrentMove] = useState(0);
  const xIsNext = currentMove % 2 === 0;
  const currentSquares = history[currentMove];

  async function handleCreateNewGame() {
    try {
      const token = getCookie("tokenX");
      const response = await fetch("https://localhost:7020/api/Game", {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          xPlayerId: xPlayer?.id,
          oPlayerId: oPlayer?.id,
        }),
      });

      if (!response.ok) {
        console.error("Error");
      } else {
        const id = await response.json();
        setCurrentGame({
          id: id,
          xPlayerId: xPlayer?.id,
          oPlayerId: oPlayer?.id,
          step: [Array(9).fill(null)],
          winner: null,
        });
        setHistory([Array(9).fill(null)]);
        setCurrentMove(0);
      }
    } catch (e) {
      console.error(e);
    }
  }

  function handleChooseGame(game: IGame) {
    setCurrentGame(game);
    setHistory(game.step || [Array(9).fill(null)]);
    setCurrentMove(game.step?.length - 1 || 0);
  }

  async function handlePlay(nextSquares: string[]) {
    const nextHistory = [...history.slice(0, currentMove + 1), nextSquares];
    setHistory(nextHistory);
    setCurrentMove(nextHistory.length - 1);

    try {
      const token = getCookie(xIsNext ? "tokenX" : "tokenO");
      const response = await fetch(
        `https://localhost:7020/api/Game/${currentGame?.id}`,
        {
          method: "PUT",
          credentials: "include",
          headers: {
            "Content-Type": "application/json; charset=utf-8",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            step: nextHistory,
            winner: calculateWinner(nextSquares),
          }),
        }
      );

      if (!response.ok) {
        setCurrentMove(nextHistory.length >= 2 ? nextHistory.length - 2 : 0);
        setHistory((prev) => [...prev.slice(0, currentMove + 1)]);

        console.error("Error");
      } else {
        setGames((prev) =>
          prev.map((game) =>
            game.id !== currentGame?.id
              ? game
              : {
                  ...game,
                  step: nextHistory,
                  winner: calculateWinner(nextSquares),
                }
          )
        );
      }
    } catch (e) {
      setCurrentMove(nextHistory.length >= 2 ? nextHistory.length - 2 : 0);
      setHistory((prev) => [...prev.slice(0, currentMove + 1)]);

      console.error(e);
    }
  }

  function jumpTo(nextMove: number) {
    setCurrentMove(nextMove);
  }

  const moves = history.map((_, move) => {
    let description;
    if (move > 0) {
      description = "Go to move #" + move;
    } else {
      description = "Go to game start";
    }
    return (
      <li key={move}>
        <button onClick={() => jumpTo(move)}>{description}</button>
      </li>
    );
  });

  useEffect(() => {
    async function GetPlayingGames() {
      try {
        const token = getCookie("tokenX");
        const response = await fetch(
          `https://localhost:7020/api/Game?xPlayerId=${xPlayer?.id}&oPlayerId=${oPlayer?.id}&winner=null`,
          {
            method: "GET",
            credentials: "include",
            headers: {
              "Content-Type": "application/json; charset=utf-8",
              Authorization: `Bearer ${token}`,
            },
          }
        );
        if (!response.ok) {
          console.error("Error");
        } else {
          const data = await response.json();
          setGames(data);
        }
      } catch (e) {
        console.error(e);
      }
    }

    if (!!xPlayer && !!oPlayer) {
      GetPlayingGames();
    } else {
      setGames([]);
      setCurrentGame(null);
      setCurrentMove(0);
      setHistory([Array(9).fill(null)]);
    }
  }, [xPlayer, oPlayer]);

  return (
    <main>
      <div className="game">
        <div>
          <LoginBoard side="X" player={xPlayer} setPlayer={setXPlayer} />
        </div>
        <div className="game-board">
          <Board
            currentGameId={currentGame?.id || null}
            xIsNext={xIsNext}
            squares={currentSquares}
            onPlay={handlePlay}
            onNewGame={handleCreateNewGame}
          />
          <button
            onClick={() => {
              setCurrentMove(0);
              setHistory([Array(9).fill(null)]);
            }}
          >
            Reset
          </button>
          <div className="game-info">
            <ol>{moves}</ol>
          </div>
        </div>
        <div>
          <LoginBoard side="O" player={oPlayer} setPlayer={setOPlayer} />
        </div>
      </div>
      <div>
        <GameList gameList={games || []} onChoose={handleChooseGame} />
      </div>
    </main>
  );
}

export default App;
