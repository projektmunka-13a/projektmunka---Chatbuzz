// =====================================================================
// Feed.jsx – a hírfolyam (főoldal), egyelőre csak kinézet kitalált adatokkal.
// Felépítés: bal oldalt menü | középen új poszt + posztok | jobb oldalt ajánlott felhasználók
// Az összekötéskor a mockData helyére az api.getPosts() és api.createPost() kerül.
// =====================================================================
import { useState } from "react";
import "./Feed.css";
import Icon from "../components/Icon";
import Avatar from "../components/Avatar";
import Post from "../components/Post";
import { me, initialPosts, suggestions } from "../mockData";
 
// A bal oldali menü pontjai. Az "active" a kiemelt (jelenlegi) oldal.
const NAV = [
  { icon: "home", label: "Főoldal", active: true },
  { icon: "search", label: "Keresés" },
  { icon: "message", label: "Üzenetek" },
  { icon: "bell", label: "Értesítések" },
  { icon: "user", label: "Profil" },
];
 
// onLogout: az App-tól kapott függvény, ami visszavisz a bejelentkezésre.
export default function Feed({ onLogout }) {
  const [posts, setPosts] = useState(initialPosts); // a megjelenített posztok
  const [content, setContent] = useState("");       // az új poszt szövege
  const [following, setFollowing] = useState({});   // kit követünk: { felhasználóId: true/false }
 
  // Új poszt "közzététele": csak a helyi listához adjuk hozzá, legelőre.
  function handleSubmit(e) {
    e.preventDefault(); // ne töltse újra az oldalt
    const text = content.trim();
    if (!text) return; // üres posztot nem teszünk közzé
    const newPost = {
      id: Date.now(), // ideiglenes egyedi azonosító
      userId: me.id,
      username: me.username,
      profilePic: null,
      content: text,
      imageUrl: null,
      createdAt: new Date().toISOString(),
      likeCount: 0,
      commentCount: 0,
    };
    setPosts([newPost, ...posts]);
    setContent("");
  }
 
  // Követés ki/be kapcsolása egy felhasználónál.
  const toggleFollow = (id) => setFollowing({ ...following, [id]: !following[id] });
 
  return (
    <div className="feed-page">
      {/* ---------- Bal oldal: menü ---------- */}
      <aside className="sidebar">
        <img className="sidebar-logo" src="/chatbuzz-icon-cb.svg" alt="ChatBuzz" />
        <nav className="nav">
          {NAV.map((item) => (
            <button key={item.label} className={`nav-item ${item.active ? "active" : ""}`}>
              <Icon name={item.icon} />
              <span>{item.label}</span>
            </button>
          ))}
        </nav>
        <div className="sidebar-bottom">
          <button className="nav-item" onClick={onLogout}>
            <Icon name="logout" />
            <span>Kijelentkezés</span>
          </button>
        </div>
      </aside>
 
      {/* ---------- Közép: új poszt + posztok ---------- */}
      <main className="feed-main">
        <h1 className="feed-title">Főoldal</h1>
 
        {/* Új poszt írása */}
        <form className="card composer" onSubmit={handleSubmit}>
          <Avatar username={me.username} id={me.id} />
          <textarea
            placeholder="Mi jár a fejedben?"
            aria-label="Új poszt szövege"
            rows={2}
            value={content}
            onChange={(e) => setContent(e.target.value)}
          />
          <div className="composer-actions">
            {/* Addig tiltott, amíg nincs beírva szöveg */}
            <button className="publish-btn" disabled={content.trim() === ""}>
              Közzététel
            </button>
          </div>
        </form>
 
        {/* A posztok: mindegyik egy Post kártya */}
        {posts.map((p) => (
          <Post key={p.id} post={p} />
        ))}
      </main>
 
      {/* ---------- Jobb oldal: saját profil + ajánlott felhasználók (kis képernyőn rejtve) ---------- */}
      <aside className="rightbar">
        <div className="me-card">
          <Avatar username={me.username} id={me.id} size={48} />
          <strong>{me.username}</strong>
        </div>
 
        <section className="card suggest">
          <h2>Ajánlott felhasználók</h2>
          <ul>
            {suggestions.map((s) => (
              <li key={s.id}>
                <Avatar username={s.username} id={s.id} size={36} />
                <span className="name">{s.username}</span>
                <button
                  className={`follow-btn ${following[s.id] ? "on" : ""}`}
                  onClick={() => toggleFollow(s.id)}
                >
                  {following[s.id] ? "Követed" : "Követés"}
                </button>
              </li>
            ))}
          </ul>
        </section>
      </aside>
    </div>
  );
}