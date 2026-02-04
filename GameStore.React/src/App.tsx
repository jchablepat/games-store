import { BrowserRouter, Route, Routes } from "react-router-dom";
import GameList from "./components/GameList";
import GameForm from "./components/GameForm";
import "./App.css";

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-900 text-white">
        <Routes>
          <Route path="/" element={<GameList />} />
          <Route path="/new" element={<GameForm />} />
          <Route path="/edit/:id" element={<GameForm />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
