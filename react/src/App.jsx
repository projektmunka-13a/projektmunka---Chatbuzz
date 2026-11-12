// =====================================================================
// App.jsx – az alkalmazás gyökere: eldönti, melyik oldal látszik.
// Egyelőre csak a kinézet készül, ezért az oldalak között állapottal
// váltunk: bejelentkezés <-> regisztráció, illetve a hírfolyam.
// (Router nem kell hozzá.)
// =====================================================================
import { useState } from "react";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Feed from "./pages/Feed";
 
export default function App() {
  // Melyik oldal látszik éppen: "login", "register" vagy "feed".
  const [page, setPage] = useState("login");
 
  // A hírfolyam: a Kijelentkezés gomb visszavisz a bejelentkezéshez.
  if (page === "feed") return <Feed onLogout={() => setPage("login")} />;
 
  // Bejelentkezés: a Bejelentkezés gomb egyelőre a hírfolyamra visz (demó, nincs ellenőrzés).
  // Regisztráció: az onShow... függvényeket átadjuk, hogy a gombjaik válthassanak.
  return page === "login" ? (
    <Login onShowRegister={() => setPage("register")} onLogin={() => setPage("feed")} />
  ) : (
    <Register onShowLogin={() => setPage("login")} />
  );
}