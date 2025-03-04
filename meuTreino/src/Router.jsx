import { Routes, Route } from "react-router-dom";
import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register";
import Home from "./pages/Home/Home";
import ExerciseDetail from "./pages/ExerciseDetail/ExerciseDetail";



export function Router() {
    return(
        <Routes>
                <Route path="/" element={<Login />}/>
                <Route path="/cadastro" element={<Register />}/>
                <Route path="/home" element={<Home />}/>
                <Route path="/home/:id" element={<ExerciseDetail />} /> 
              
        </Routes>
    )
}
