import React from "react";
import { MenuBase } from "../../Shared/Menu/MenuBase";

export default class EntradasPage extends MenuBase {
  getTitle(): string { return "Entradas"; }
  renderContent(): React.ReactNode {
    return <div>Lista de entradas (em breve)</div>;
  }
}

