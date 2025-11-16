import axios from "axios";
import User from "../Models/User";

const API_BASE = (process.env.REACT_APP_API_BASE as string) || "http://localhost:5209";

// Mais simples: apenas verificar se o usuário existe e se a senha bate
export async function verifyCredentials(login: string, senha: string) {
  const url = `${API_BASE}/api/GetUsuarioByLogin=${encodeURIComponent(login)}`;
  try {
    const resp = await axios.get<User>(url, { validateStatus: () => true });
    if (resp.status === 404) return { ok: false, message: "#MSG0002 - Usuário não encontrado!" };
    if (resp.status >= 400) return { ok: false, message: (resp.data as any) || "Falha ao autenticar" };

    const user = resp.data;
    if (!user || user.senha !== senha) return { ok: false, message: "#MSG0099 - Usuário ou senha inválidos" };
    return { ok: true, user };
  } catch (e: any) {
    return { ok: false, message: `#FRONT - ${e?.message ?? "Falha de rede"}` };
  }
}

