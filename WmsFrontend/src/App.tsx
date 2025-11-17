import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginForm from "./Views/LoginPage/Components/LoginForm";
import HomePage from "./Views/HomePage/Components/HomePage";

import "./index.css";

function isAuthenticated() {
  try {
    return !!(localStorage.getItem("wms-user") || sessionStorage.getItem("wms-user"));
  } catch {
    return false;
  }
}

function Protected({ children }: { children: React.ReactElement }) {
  const auth = isAuthenticated();
  console.log('Protected route - authenticated:', auth);
  return auth ? children : <Navigate to="/" replace />;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/home" element={<Protected><HomePage /></Protected>} />

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
