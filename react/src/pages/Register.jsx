// =====================================================================
// Register.jsx – a regisztráció oldal (egyelőre csak kinézet).
// Kötelező: e-mail-cím, felhasználónév, jelszó.
// =====================================================================
import { useState } from "react";
import AuthLayout from "../components/AuthLayout"; // közös keret
 
// Egyszerű e-mail ellenőrzés: nincs szóköz, van @ és egy pont utána (pl. a@b.hu).
const EMAIL_RE = /^\S+@\S+\.\S+$/;
 
// onShowLogin: az App-tól kapott függvény, ami visszavált a bejelentkezésre.
export default function Register({ onShowLogin }) {
  // A három beírt adat állapota:
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
 
  // A gomb akkor aktív, ha az e-mail formátuma helyes ÉS a másik két mező nem üres.
  const ready = EMAIL_RE.test(email.trim()) && username.trim() !== "" && password !== "";
 
  // Az űrlap elküldésekor fut le.
  function handleSubmit(e) {
    e.preventDefault(); // ne töltse újra az oldalt a böngésző
    // Később ide jön az összekötés: api.register(username, email, password)
  }
 
  return (
    <AuthLayout>
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>Fiók létrehozása a ChatBuzzon</h2>
 
        {/* E-mail mező: type="email" miatt a mobil billentyűzet is e-mailre állítja magát. */}
        <input
          type="email"
          placeholder="E-mail-cím"
          aria-label="E-mail-cím"
          autoComplete="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
 
        {/* Felhasználónév mező */}
        <input
          type="text"
          placeholder="Felhasználónév"
          aria-label="Felhasználónév"
          autoComplete="username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
 
        {/* Jelszó mező. A "new-password" azt jelzi a böngészőnek, hogy ÚJ jelszót adunk meg
            (felajánlhat erős jelszót, és nem a régit tölti ki). */}
        <input
          type="password"
          placeholder="Jelszó"
          aria-label="Jelszó"
          autoComplete="new-password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
 
        {/* Fő gomb: addig tiltott, amíg az adatok nem teljesek és helyesek */}
        <button type="submit" className="btn btn-primary" disabled={!ready}>
          Fiók létrehozása
        </button>
 
        <div className="divider"><span>vagy</span></div>
 
        {/* Visszavált a bejelentkezésre */}
        <button type="button" className="btn btn-outline" onClick={onShowLogin}>
          Van már fiókom, bejelentkezem
        </button>
      </form>
    </AuthLayout>
  );
}