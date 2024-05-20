export interface IGame {
  id: number;
  xPlayerId: number | null;
  oPlayerId: number | null;
  step: Array<string[]>;
  winner: "X" | "O" | "Draw" | null;
}
