import React from "react";
import "./button.css";

type Props = {
  children: React.ReactNode;
  onClick?: () => void;
  type?: "button" | "submit";
  disabled?: boolean;
};

export default function Button({ children, onClick, type = "button", disabled }: Props) {
  return (
    <button className="lp-btn" type={type} onClick={onClick} disabled={disabled}>
      {children}
    </button>
  );
}

