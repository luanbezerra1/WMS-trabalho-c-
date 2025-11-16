import React from "react";
import { MenuBase } from "../../Shared/Menu/MenuBase";

export default class SaidasPage extends MenuBase {
  getTitle(): string { return "Saídas"; }
  renderContent(): React.ReactNode {
    return <div>Lista de saídas (em breve)</div>;
  }
}

