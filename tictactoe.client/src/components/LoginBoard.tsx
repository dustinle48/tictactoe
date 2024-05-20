import { Dispatch, FormEvent, SetStateAction } from "react";
import { User } from "../interfaces/IUser";
import UserForm from "./UserForm";
import { deleteCookie } from "../utils/cookieUtils";

type LoginBoardProps = {
  side: "X" | "O";
  player: User | null;
  setPlayer: Dispatch<SetStateAction<User | null>>;
};

function LoginBoard(props: LoginBoardProps) {
  const { side, player, setPlayer } = props;

  const gameCount = player?.games.length;

  const winCount = player?.games.filter(
    (game) =>
      (game.xPlayerId === player?.id && game.winner === "X") ||
      (game.oPlayerId === player?.id && game.winner === "O")
  ).length;

  const winrate = (winCount / gameCount) * 100 || "No data";

  async function handleLogin(event: FormEvent) {
    event.preventDefault();

    const data = {
      side: side,
      name: (event.target as HTMLFormElement).name.value,
      password: (event.target as HTMLFormElement).password.value,
    };

    try {
      const response = await fetch(`https://localhost:7020/api/User/login`, {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        const user = await response.json();
        setPlayer(user);
      } else {
        console.error("Login failed");
      }
    } catch (e) {
      console.error(e);
    }
  }

  async function handleLogup(event: FormEvent) {
    event.preventDefault();

    const data = {
      name: (event.target as HTMLFormElement).name.value,
      password: (event.target as HTMLFormElement).password.value,
    };

    try {
      const response = await fetch(`https://localhost:7020/api/User`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json; charset=utf-8",
        },
        body: JSON.stringify(data),
      });

      //If Sign up sucess, login with new user
      if (response.ok) {
        const _res = await fetch(`https://localhost:7020/api/User/login`, {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json; charset=utf-8",
          },
          body: JSON.stringify(data),
        });

        const user = await _res.json();
        setPlayer(user);
      } else {
        console.error("Name already exists");
      }
    } catch (e) {
      console.error(e);
    }
  }

  return (
    <div className="login-board">
      {player ? (
        <div>
          <p>Welcome {player.name}!</p>
          <p>You played: {gameCount} game(s).</p>
          <p>Your winrate: {winrate}%</p>
          <button
            onClick={() => {
              deleteCookie(`token${side}`);
              setPlayer(null);
            }}
          >
            Logout
          </button>
        </div>
      ) : (
        <>
          <div className="login-form">
            <UserForm label="Log In" onSubmit={handleLogin} />
          </div>
          <div className="signup-form">
            <UserForm label="Sign Up" onSubmit={handleLogup} />
          </div>
        </>
      )}
    </div>
  );
}

export default LoginBoard;
