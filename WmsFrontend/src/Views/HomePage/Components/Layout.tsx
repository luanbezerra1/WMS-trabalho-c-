import React, { useState } from "react";
import "../Styles/home.css";

export default function Layout({ children }: { children?: React.ReactNode }) {
  const [open, setOpen] = useState(true);
  return (
    <div className={`home-root ${open ? "open" : "closed"}`}>
      <aside className="sidebar">
        <div className="brand">WMS</div>
        <nav className="menu">
          <a className="menu-item" href="#">Dashboard</a>
          <a className="menu-item" href="#">Produtos</a>
          <a className="menu-item" href="#">Clientes</a>
          <a className="menu-item" href="#">Fornecedores</a>
        </nav>
      </aside>
      <main className="content">
        <header className="topbar">
          <button className="toggle" onClick={() => setOpen(v => !v)}>{open ? "⟨" : "⟩"}</button>
          <div className="spacer" />
          <div className="user">Bem-vindo</div>
        </header>
        <div className="page">{children ?? <div className="placeholder">Home</div>}</div>
      </main>
    </div>
  );
}

