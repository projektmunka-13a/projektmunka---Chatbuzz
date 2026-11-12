// =====================================================================
// Login.jsx – a bejelentkezés oldal (egyelőre csak kinézet).
// Belépni felhasználónévvel VAGY e-mail-címmel lehet + jelszó.
// =====================================================================
import { useState } from "react";
import AuthLayout from "../components/AuthLayout"; // közös keret
 
// onShowRegister: az App-tól kapott függvény, ami a regisztrációra vált.
// onLogin: az App-tól kapott függvény, ami a hírfolyamra lép (egyelőre demó).
export default function Login({ onShowRegister, onLogin }) {
  // Amit a felhasználó beírt (a React "állapotként" tárolja):
  const [identifier, setIdentifier] = useState(""); // felhasználónév vagy e-mail
  const [password, setPassword] = useState("");
 
  // A gomb csak akkor aktív, ha mindkét mező ki van töltve
  // (a trim() levágja a felesleges szóközöket).
  const ready = identifier.trim() !== "" && password !== "";
 
  // Az űrlap elküldésekor fut le (Enter vagy a gomb).
  function handleSubmit(e) {
    e.preventDefault(); // megakadályozza, hogy a böngésző újratöltse az oldalt
    // Egyelőre demó: nincs ellenőrzés, a hírfolyamra lép.
    // Később ide jön az összekötés: api.login(identifier, password), és csak siker esetén lépünk tovább.
    onLogin?.();
  }
 
  return (
    <AuthLayout>
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>Bejelentkezés a ChatBuzzra</h2>
 
        {/* Felhasználónév vagy e-mail mező.
            value + onChange: a mező mindig az állapotot mutatja, és gépeléskor frissíti azt.
            aria-label: a képernyőolvasónak (a placeholder nem mindig elég).
            autoComplete: a böngésző jelszókezelője ebből tudja, mit töltsön ki. */}
        <input
          type="text"
          placeholder="Felhasználónév vagy e-mail-cím"
          aria-label="Felhasználónév vagy e-mail-cím"
          autoComplete="username"
          value={identifier}
          onChange={(e) => setIdentifier(e.target.value)}
        />
 
        {/* Jelszó mező: type="password" miatt a karakterek pontokként látszanak. */}
        <input
          type="password"
          placeholder="Jelszó"
          aria-label="Jelszó"
          autoComplete="current-password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
 
        {/* Fő gomb: a disabled miatt addig halvány és kattinthatatlan, amíg nincs minden kitöltve. */}
        <button type="submit" className="btn btn-primary" disabled={!ready}>
          Bejelentkezés
        </button>
 
        {/* "vagy" elválasztó vonal */}
        <div className="divider"><span>vagy</span></div>
 
        {/* Átvált a regisztrációra. type="button": ne küldje el az űrlapot. */}
        <button type="button" className="btn btn-outline" onClick={onShowRegister}>
          Új fiók létrehozása
        </button>
      </form>
    </AuthLayout>
  );
}