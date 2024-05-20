import { IGame } from "./IGame";

export interface User {
  id: number;
  name: string;
  password: string;
  games: IGame[];
}
