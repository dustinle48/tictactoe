type SquareProps = {
  value: string;
  onSquareClick: () => void;
};

function Square(props: SquareProps) {
  const { value, onSquareClick } = props;

  return (
    <button className="square" onClick={onSquareClick}>
      {value}
    </button>
  );
}

export default Square;
