import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { verifyCredentials } from "../Controllers/authController";
import "../Styles/login.css";

export default function LoginForm() {
  const [login, setLogin] = useState("");
  const [senha, setSenha] = useState("");
  const [remember, setRemember] = useState(false);
  const [loading, setLoading] = useState(false);
  const [toast, setToast] = useState<{ kind: "success" | "error"; text: string } | null>(null);
  const navigate = useNavigate();

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    const res = await verifyCredentials(login.trim(), senha);
    setLoading(false);
    if (res.ok && res.user) {
      const storage = remember ? localStorage : sessionStorage;
      storage.setItem("wms-user", JSON.stringify({ id: res.user.id, login: res.user.login, nome: res.user.nome }));
      setToast({ kind: "success", text: "#LOGIN - Login efetuado com sucesso!" });
      setTimeout(() => {
        setToast(null);
        navigate("/ordens");
      }, 1000);
    } else {
      setToast({ kind: "error", text: res.message || "Falha ao autenticar" });
      setTimeout(() => setToast(null), 2500);
    }
  }

  return (
    <div className="login-container">
      {toast && <div className={`toast ${toast.kind === "error" ? "error" : ""}`}>{toast.text}</div>}
      <div className="login-card">
        <div className="login-header"><div className="brand brand-wms">WMS</div></div>
        <form onSubmit={onSubmit}>
          <div className="lp-field">
            <label className="lp-label" htmlFor="usuario">Usuário <span className="lp-req">*</span></label>
            <input
              id="usuario"
              className="lp-input"
              placeholder="userName"
              value={login}
              onChange={(e) => setLogin(e.target.value)}
              autoComplete="username"
              required
            />
          </div>
          <div className="lp-field">
            <label className="lp-label" htmlFor="senha">Senha <span className="lp-req">*</span></label>
            <input
              id="senha"
              type="password"
              className="lp-input"
              placeholder="••••••••"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              autoComplete="current-password"
              required
            />
          </div>
          <label className="remember">
            <input type="checkbox" checked={remember} onChange={e => setRemember(e.target.checked)} /> Lembrar de mim
          </label>
          <button className="lp-btn" type="submit" disabled={loading}>{loading ? "Entrando..." : "Entrar"}</button>
        </form>
      </div>
    </div>
  );
}

