import React from "react";
import { MenuBase } from "../../Shared/Menu/MenuBase";

export default class OrdensPage extends MenuBase {
  getTitle(): string { return "Ordens"; }
  renderContent(): React.ReactNode {
    return (
      <div>
        <div style={{display:'flex',alignItems:'center',justifyContent:'space-between',marginBottom:12}}>
          <h2 style={{margin:0,fontSize:16}}>Últimas Ordens</h2>
          <div style={{display:'flex',gap:8}}>
            <button className="lp-btn" style={{height:36}}>Nova Saída</button>
            <button className="lp-btn" style={{height:36}}>Nova Entrada</button>
          </div>
        </div>
        <div style={{overflowX:'auto',border:'1px solid #e6ebf2',borderRadius:8}}>
          <table style={{width:'100%',borderCollapse:'separate',borderSpacing:0}}>
            <thead style={{background:'#f9fbff'}}>
              <tr>
                <th style={{textAlign:'left',padding:'10px 12px',borderBottom:'1px solid #e6ebf2'}}>#</th>
                <th style={{textAlign:'left',padding:'10px 12px',borderBottom:'1px solid #e6ebf2'}}>Tipo</th>
                <th style={{textAlign:'left',padding:'10px 12px',borderBottom:'1px solid #e6ebf2'}}>Produto/Cliente</th>
                <th style={{textAlign:'left',padding:'10px 12px',borderBottom:'1px solid #e6ebf2'}}>Quantidade</th>
                <th style={{textAlign:'left',padding:'10px 12px',borderBottom:'1px solid #e6ebf2'}}>Data</th>
                <th style={{textAlign:'left',padding:'10px 12px',borderBottom:'1px solid #e6ebf2'}}>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td style={{padding:'10px 12px'}}>1001</td>
                <td style={{padding:'10px 12px'}}>Entrada</td>
                <td style={{padding:'10px 12px'}}>Produto A</td>
                <td style={{padding:'10px 12px'}}>50</td>
                <td style={{padding:'10px 12px'}}>2025-11-16</td>
                <td style={{padding:'10px 12px'}}>Concluída</td>
              </tr>
              <tr>
                <td style={{padding:'10px 12px'}}>1002</td>
                <td style={{padding:'10px 12px'}}>Saída</td>
                <td style={{padding:'10px 12px'}}>Cliente X</td>
                <td style={{padding:'10px 12px'}}>12</td>
                <td style={{padding:'10px 12px'}}>2025-11-16</td>
                <td style={{padding:'10px 12px'}}>Pendente</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    );
  }
}

