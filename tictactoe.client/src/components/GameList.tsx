import { IGame } from "../interfaces/IGame";

type GameProps = {
  gameInfo: IGame;
  onChoose: (game: IGame) => void;
};

function Game(props: GameProps) {
  const { gameInfo, onChoose } = props;

  return (
    <button onClick={() => onChoose(gameInfo)}>{`${gameInfo.id} - At #${
      gameInfo.step?.length || "0"
    } move`}</button>
  );
}

type GameListProps = {
  gameList: IGame[];
  onChoose: (game: IGame) => void;
  onNewGame: () => void;
};

function GameList(props: GameListProps) {
  const { gameList, onChoose, onNewGame } = props;

  return (
    <div>
      {gameList.map((game) => (
        <Game gameInfo={game} onChoose={onChoose} />
      ))}
      <button onClick={onNewGame}>New game</button>
    </div>
  );
}

export default GameList;
