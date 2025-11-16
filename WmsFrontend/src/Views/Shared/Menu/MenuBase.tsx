import React from "react";
import { NavLink } from "react-router-dom";
import { MenuItem, defaultMenu } from "./MenuItem";
import "./menu.css";

export abstract class MenuBase<P = unknown, S = unknown> extends React.Component<P, S> {
  // Título exibido no topo
  abstract getTitle(): string;
  // Conteúdo principal da página
  abstract renderContent(): React.ReactNode;
  // Itens de menu - por padrão usa os padrões, mas pode sobrescrever
  getMenuItems(): MenuItem[] { return defaultMenu; }

  render(): React.ReactNode {
    const items = this.getMenuItems();
    return (
      <div className="layout-root">
        <aside className="layout-sidebar">
          <div className="layout-brand">WMS</div>
          <nav className="layout-menu">
            {items.map(i => (
              <NavLink key={i.path} to={i.path} className={({isActive}) => "layout-link" + (isActive ? " active" : "")}>
                {i.label}
              </NavLink>
            ))}
          </nav>
        </aside>
        <main className="layout-main">
          <header className="layout-header">
            <h1 className="layout-title">{this.getTitle()}</h1>
          </header>
          <section className="layout-page">
            {this.renderContent()}
          </section>
        </main>
      </div>
    );
  }
}

