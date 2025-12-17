export default function IconButton({ children, onClick,}: {children: React.ReactNode; onClick?: () => void; }) {
  return (
    <button
      onClick={onClick}
      className="p-2.5 hover:bg-gray-50 rounded-full transition-all duration-200"
    >
      {children}
    </button>
  );
}
