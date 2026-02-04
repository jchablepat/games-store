import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { Game } from "../models/types";
import { getGames, deleteGame } from "../services/gameService";

function GameList() {
    const [games, setGames] = useState<Game[]>([]);

    useEffect(() => {
        loadGames();
    }, []);

    const loadGames = async () => {
        try {
            const data = await getGames();
            setGames(data);
        } catch (error) {
            console.error("Error loading games", error);
        }
    };

    const handleDelete = async (id: number) => {
        if (window.confirm("¿Estás seguro de que quieres eliminar este juego?")) {
            try {
                await deleteGame(id);
                loadGames();
            } catch (error) {
                console.error("Error deleting game", error);
            }
        }
    };

    return (
        <div className="container mx-auto p-4">
            <h1 className="text-4xl font-bold mb-8 text-center text-transparent bg-clip-text bg-gradient-to-r from-purple-400 to-pink-600">
                Game Store
            </h1>
            
            <div className="flex justify-end mb-6">
                <Link 
                    to="/new" 
                    className="bg-gradient-to-r from-green-400 to-blue-500 hover:from-green-500 hover:to-blue-600 text-white font-bold py-2 px-6 rounded-full shadow-lg transform hover:scale-105 transition duration-300"
                >
                    + Nuevo Juego
                </Link>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {games.map((game) => (
                    <div key={game.id} className="bg-gray-800 rounded-xl overflow-hidden shadow-xl hover:shadow-2xl transition duration-300 border border-gray-700 flex flex-col">
                        {game.thumbnail && (
                            <img src={game.thumbnail} alt={game.title} className="w-full h-48 object-cover" />
                        )}
                        <div className="p-6 flex-grow">
                            <h2 className="text-2xl font-bold mb-2 text-white">{game.title}</h2>
                            <p className="text-purple-400 font-semibold mb-2">{game.genre}</p>
                            
                            {game.description && (
                                <p className="text-gray-400 text-sm mb-4 line-clamp-3">
                                    {game.description}
                                </p>
                            )}

                            <div className="space-y-1 mb-4 text-xs text-gray-500">
                                {game.platform && <p>Plataforma: <span className="text-gray-300">{game.platform}</span></p>}
                                {game.publisher && <p>Publisher: <span className="text-gray-300">{game.publisher}</span></p>}
                                {game.developer && <p>Desarrollador: <span className="text-gray-300">{game.developer}</span></p>}
                            </div>

                            <div className="flex justify-between items-center mt-auto">
                                <span className="text-3xl font-bold text-green-400">${game.price}</span>
                                <span className="text-sm text-gray-500">{new Date(game.releaseDate).toLocaleDateString()}</span>
                            </div>
                        </div>
                        <div className="bg-gray-900 px-6 py-4 flex justify-between items-center">
                            <Link 
                                to={`/edit/${game.id}`}
                                className="text-blue-400 hover:text-blue-300 font-semibold"
                            >
                                Editar
                            </Link>
                            <button 
                                onClick={() => handleDelete(game.id)}
                                className="text-red-400 hover:text-red-300 font-semibold"
                            >
                                Eliminar
                            </button>
                        </div>
                    </div>
                ))}
            </div>
            
            {games.length === 0 && (
                <div className="text-center text-gray-500 mt-12">
                    <p className="text-xl">No hay juegos registrados.</p>
                </div>
            )}
        </div>
    );
}

export default GameList;
