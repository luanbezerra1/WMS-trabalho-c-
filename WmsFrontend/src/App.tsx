import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginForm from "./Views/LoginPage/Components/LoginForm";
import OrdensPage from "./Views/OrdensPage/Components/OrdensPage";
import EntradasPage from "./Views/EntradasPage/Components/EntradasPage";
import SaidasPage from "./Views/SaidasPage/Components/SaidasPage";
import "./index.css";

function isAuthenticated() {
  try {
    return !!(localStorage.getItem("wms-user") || sessionStorage.getItem("wms-user"));
  } catch {
    return false;
  }
}

function Protected({ children }: { children: React.ReactElement }) {
  return isAuthenticated() ? children : <Navigate to="/" replace />;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/ordens" element={<Protected><OrdensPage /></Protected>} />
        <Route path="/entradas" element={<Protected><EntradasPage /></Protected>} />
        <Route path="/saidas" element={<Protected><SaidasPage /></Protected>} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
