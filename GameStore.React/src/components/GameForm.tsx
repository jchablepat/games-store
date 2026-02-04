import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { CreateGame, Genre } from "../models/types";
import { createGame, getGame, getGenres, updateGame } from "../services/gameService";

function GameForm() {
    const { id } = useParams();
    const navigate = useNavigate();
    const isEditing = !!id;

    const [game, setGame] = useState<CreateGame>({
        title: "",
        genreId: 0,
        price: 0,
        releaseDate: new Date().toISOString().split('T')[0],
        thumbnail: "",
        description: "",
        platform: "",
        publisher: "",
        developer: ""
    });

    const [genres, setGenres] = useState<Genre[]>([]);

    useEffect(() => {
        loadGenres();
        if (isEditing) {
            loadGame(Number(id));
        }
    }, [id]);

    const loadGenres = async () => {
        try {
            const data = await getGenres();
            setGenres(data);
        } catch (error) {
            console.error("Error loading genres", error);
        }
    };

    const loadGame = async (gameId: number) => {
        try {
            const data = await getGame(gameId);
            setGame({
                title: data.title,
                genreId: data.genreId,
                price: data.price,
                releaseDate: data.releaseDate.split('T')[0],
                thumbnail: data.thumbnail || "",
                description: data.description || "",
                platform: data.platform || "",
                publisher: data.publisher || "",
                developer: data.developer || ""
            });
        } catch (error) {
            console.error("Error loading game", error);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            if (isEditing) {
                await updateGame(Number(id), game);
            } else {
                await createGame(game);
            }
            navigate("/");
        } catch (error) {
            console.error("Error saving game", error);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setGame(prev => ({
            ...prev,
            [name]: name === "genreId" || name === "price" ? Number(value) : value
        }));
    };

    return (
        <div className="container mx-auto p-4 max-w-lg">
            <h1 className="text-3xl font-bold mb-6 text-center text-white">
                {isEditing ? "Editar Juego" : "Nuevo Juego"}
            </h1>
            
            <form onSubmit={handleSubmit} className="bg-gray-800 p-8 rounded-xl shadow-2xl border border-gray-700">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
                    <div>
                        <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="platform">
                            Plataforma
                        </label>
                        <input
                            type="text"
                            id="platform"
                            name="platform"
                            value={game.platform || ""}
                            onChange={handleChange}
                            className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        />
                    </div>
                    <div>
                        <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="publisher">
                            Publisher
                        </label>
                        <input
                            type="text"
                            id="publisher"
                            name="publisher"
                            value={game.publisher || ""}
                            onChange={handleChange}
                            className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        />
                    </div>
                    <div>
                        <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="developer">
                            Desarrollador
                        </label>
                        <input
                            type="text"
                            id="developer"
                            name="developer"
                            value={game.developer || ""}
                            onChange={handleChange}
                            className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        />
                    </div>
                </div>

                <div className="mb-4">
                    <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="title">
                        Título
                    </label>
                    <input
                        type="text"
                        id="title"
                        name="title"
                        value={game.title}
                        onChange={handleChange}
                        className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        required
                    />
                </div>

                <div className="mb-4">
                    <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="thumbnail">
                        URL de la Imagen (Thumbnail)
                    </label>
                    <input
                        type="text"
                        id="thumbnail"
                        name="thumbnail"
                        value={game.thumbnail || ""}
                        onChange={handleChange}
                        className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        placeholder="https://ejemplo.com/imagen.jpg"
                    />
                </div>

                <div className="mb-4">
                    <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="description">
                        Descripción
                    </label>
                    <textarea
                        id="description"
                        name="description"
                        value={game.description || ""}
                        onChange={handleChange}
                        className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        rows={3}
                    />
                </div>

                <div className="mb-4">
                    <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="genreId">
                        Género
                    </label>
                    <select
                        id="genreId"
                        name="genreId"
                        value={game.genreId}
                        onChange={handleChange}
                        className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        required
                    >
                        <option value={0}>Seleccione un género</option>
                        {genres.map(g => (
                            <option key={g.id} value={g.id}>{g.name}</option>
                        ))}
                    </select>
                </div>

                <div className="mb-4">
                    <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="price">
                        Precio
                    </label>
                    <input
                        type="number"
                        id="price"
                        name="price"
                        step="0.01"
                        value={game.price}
                        onChange={handleChange}
                        className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        required
                    />
                </div>

                <div className="mb-6">
                    <label className="block text-gray-300 text-sm font-bold mb-2" htmlFor="releaseDate">
                        Fecha de Lanzamiento
                    </label>
                    <input
                        type="date"
                        id="releaseDate"
                        name="releaseDate"
                        value={game.releaseDate}
                        onChange={handleChange}
                        className="w-full px-3 py-2 bg-gray-700 text-white border border-gray-600 rounded focus:outline-none focus:border-blue-500 transition-colors"
                        required
                    />
                </div>

                <div className="flex justify-between">
                    <button
                        type="button"
                        onClick={() => navigate("/")}
                        className="bg-gray-600 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline transition duration-300"
                    >
                        Cancelar
                    </button>
                    <button
                        type="submit"
                        className="bg-gradient-to-r from-purple-500 to-pink-600 hover:from-purple-600 hover:to-pink-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline transform hover:scale-105 transition duration-300"
                    >
                        Guardar
                    </button>
                </div>
            </form>
        </div>
    );
}

export default GameForm;
