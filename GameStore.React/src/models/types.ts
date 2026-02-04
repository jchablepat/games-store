export interface Game {
    id: number;
    title: string;
    genre: string;
    price: number;
    releaseDate: string;
    thumbnail?: string;
    description?: string;
    platform?: string;
    publisher?: string;
    developer?: string;
}

export interface GameDetails {
    id: number;
    title: string;
    genreId: number;
    price: number;
    releaseDate: string;
    thumbnail?: string;
    description?: string;
    platform?: string;
    publisher?: string;
    developer?: string;
}

export interface CreateGame {
    title: string;
    genreId: number;
    price: number;
    releaseDate: string;
    thumbnail?: string;
    description?: string;
    platform?: string;
    publisher?: string;
    developer?: string;
}

export interface Genre {
    id: number;
    name: string;
}
