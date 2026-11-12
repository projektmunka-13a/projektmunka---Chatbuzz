// =====================================================================
// Post.jsx – egyetlen poszt kártyája a hírfolyamban.
// Props: post – a poszt adatai (PostDTO felépítés, lásd mockData.js)
// A kedvelés itt egyelőre csak helyben működik (nem megy a szerverre).
// =====================================================================
import { useState } from "react";
import Avatar from "./Avatar";
import Icon from "./Icon";
 
// Az időpontot "3 órája" jellegű szöveggé alakítja.
function timeAgo(iso) {
  const seconds = Math.max(0, (Date.now() - new Date(iso)) / 1000);
  if (seconds < 60) return "most";
  const minutes = Math.floor(seconds / 60);
  if (minutes < 60) return `${minutes} perce`;
  const hours = Math.floor(minutes / 60);
  if (hours < 24) return `${hours} órája`;
  return `${Math.floor(hours / 24)} napja`;
}
 
export default function Post({ post }) {
  const [liked, setLiked] = useState(false); // kedvelte-e a felhasználó
  const likes = post.likeCount + (liked ? 1 : 0); // a kedvelések száma a sajáttal együtt
 
  return (
    <article className="card post">
      {/* Fejléc: profilkép, név, idő */}
      <header className="post-head">
        <Avatar username={post.username} id={post.userId} src={post.profilePic} />
        <div>
          <strong>{post.username}</strong>
          <span className="post-time">{timeAgo(post.createdAt)}</span>
        </div>
      </header>
 
      {/* Szöveg és kép csak akkor jelenik meg, ha van */}
      {post.content && <p className="post-text">{post.content}</p>}
      {post.imageUrl && <img className="post-img" src={post.imageUrl} alt="" />}
 
      {/* Műveletek: kedvelés és hozzászólás (a számlálókkal) */}
      <footer className="post-actions">
        <button
          className={`action ${liked ? "liked" : ""}`}
          onClick={() => setLiked(!liked)}
          aria-pressed={liked} // a képernyőolvasónak: be van-e kapcsolva
        >
          <Icon name="heart" size={22} filled={liked} />
          <span>{likes}</span>
        </button>
        <button className="action">
          <Icon name="message" size={22} />
          <span>{post.commentCount}</span>
        </button>
      </footer>
    </article>
  );
}