import React, { useEffect, useState } from "react";
import LogsModel from "../Models/Logs";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Logs.css";
import { useRefresh } from "../Contexts/RefreshContext";

function Logs() {
  const [logs, setLogs] = useState<LogsModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { setRefreshFunction } = useRefresh();

  async function listarLogsAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<LogsModel[]>(
        "http://localhost:5209/api/GetLogs"
      );
      const dados = resposta.data;
      setLogs(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar logs. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  useEffect(() => {
    listarLogsAPI();
    setRefreshFunction(() => listarLogsAPI);
    
    return () => {
      setRefreshFunction(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Logs</h1>
        </div>
        <div className="loading-container">
          <p>Carregando...</p>
        </div>
      </div>
    );
  }

  return (
    <div id="componente_listar_enderecos">
      <div className="header-container">
        <h1>Listar Logs</h1>
      </div>

      {error && (
        <div className="error-container">
          {error}
        </div>
      )}

      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Data/Hora</th>
            <th>Mensagem</th>
          </tr>
        </thead>
        <tbody>
          {logs.length === 0 ? (
            <tr>
              <td colSpan={3} className="no-data">
                Nenhum log encontrado
              </td>
            </tr>
          ) : (
            logs.map((log) => (
              <tr key={log.logId}>
                <td>{log.logId}</td>
                <td>{new Date(log.dataHora).toLocaleString('pt-BR')}</td>
                <td style={{ textAlign: 'left', maxWidth: '800px', wordWrap: 'break-word' }}>{log.mensagem}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Logs;

