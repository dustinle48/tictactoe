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
};

function GameList(props: GameListProps) {
  const { gameList, onChoose } = props;

  return (
    <div style={{ border: "1px solid black" }}>
      <h4>Unfinised games:</h4>
      {gameList.map((game) => (
        <Game gameInfo={game} onChoose={onChoose} />
      ))}
    </div>
  );
}

export default GameList;
