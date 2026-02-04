import type { Game, GameDetails, CreateGame, Genre } from "../models/types";

const API_URL = import.meta.env.VITE_API_URL;

export const getGames = async (): Promise<Game[]> => {
    const response = await fetch(`${API_URL}/games`);
    if (!response.ok) throw new Error("Failed to fetch games");
    return response.json();
};

export const getGame = async (id: number): Promise<GameDetails> => {
    const response = await fetch(`${API_URL}/games/${id}`);
    if (!response.ok) throw new Error("Failed to fetch game");
    return response.json();
};

export const createGame = async (game: CreateGame): Promise<void> => {
    const response = await fetch(`${API_URL}/games`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(game),
    });
    if (!response.ok) throw new Error("Failed to create game");
};

export const updateGame = async (id: number, game: CreateGame): Promise<void> => {
    const response = await fetch(`${API_URL}/games/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(game),
    });
    if (!response.ok) throw new Error("Failed to update game");
};

export const deleteGame = async (id: number): Promise<void> => {
    const response = await fetch(`${API_URL}/games/${id}`, {
        method: "DELETE",
    });
    if (!response.ok) throw new Error("Failed to delete game");
};

export const getGenres = async (): Promise<Genre[]> => {
    const response = await fetch(`${API_URL}/genres`);
    if (!response.ok) throw new Error("Failed to fetch genres");
    return response.json();
};
