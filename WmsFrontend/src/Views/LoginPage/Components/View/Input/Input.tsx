import React from "react";
import "./input.css";

type Props = {
  id: string;
  label: string;
  type?: "text" | "password";
  placeholder?: string;
  required?: boolean;
  value: string;
  onChange: (value: string) => void;
};

export default function Input({ id, label, type = "text", placeholder, required, value, onChange }: Props) {
  return (
    <div className="lp-field">
      <label className="lp-label" htmlFor={id}>
        {label} {required && <span className="lp-req">*</span>}
      </label>
      <input
        id={id}
        className="lp-input"
        type={type}
        placeholder={placeholder}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        required={required}
      />
    </div>
  );
}

