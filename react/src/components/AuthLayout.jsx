// =====================================================================
// AuthLayout.jsx – a bejelentkezés és a regisztráció KÖZÖS kerete.
// Bal oldalt: logó, nagy szöveg, hero kép.
// Jobb oldalt: az űrlap (azt a két oldal adja át "children"-ként).
// Így a keretet csak egyszer kell megírni.
// =====================================================================
import "./AuthLayout.css"; // a hozzá tartozó stílusok
 
// A logó (ikon). A kép a react/public mappából jön: chatbuzz-icon-cb.svg
// A "className" azért kell, hogy mobilon külön stílust kaphasson (mobile-brand).
function Brand({ className = "" }) {
  return (
    <div className={`brand-mark ${className}`}>
      <img src="/chatbuzz-icon-cb.svg" alt="ChatBuzz" />
    </div>
  );
}
 
// A "children" az, amit a keret belsejébe írunk (jobb oldalon az űrlap).
export default function AuthLayout({ children }) {
  return (
    // A teljes oldal: kétoszlopos rács (CSS: .login-page)
    <div className="login-page">
      {/* ---- Bal oldal: bemutató rész (mobilon eltűnik) ---- */}
      <section className="login-hero">
        <Brand /> {/* logó bal felül */}
        <div className="hero-body">
          {/* Nagy szöveg; a két kiemelt szó külön színt kap (hl-a, hl-b) */}
          <h1>
            Oszd meg a <span className="hl-a">pillanataid</span>, és beszélgess a{" "}
            <span className="hl-b">barátaiddal</span>.
          </h1>
          {/* Hero kép a react/public mappából: chatbuzz-hero-transparent.svg
              (az alt üres, mert díszítőkép, a képernyőolvasó kihagyja) */}
          <img className="hero-art" src="/chatbuzz-hero-transparent.svg" alt="" />
        </div>
      </section>
 
      {/* ---- Jobb oldal: az űrlap helye ---- */}
      <section className="login-panel">
        <div className="panel-inner">
          {/* Mobilon, ahol a bal oldal el van rejtve, a logó ide kerül az űrlap fölé */}
          <Brand className="mobile-brand" />
          {children} {/* ide kerül a Login vagy a Register űrlapja */}
        </div>
      </section>
    </div>
  );
}
