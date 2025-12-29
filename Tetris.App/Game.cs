using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tetris.App
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _blockTexture;
        private SpriteFont _font;

        private GameState _gameState;
        private InputHandler _inputHandler;
        private Renderer _renderer;
        private float _fallTimer;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 700;
        }

        protected override void Initialize()
        {
            _gameState = new GameState();
            _inputHandler = new InputHandler();
            _renderer = new Renderer();
            _fallTimer = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _blockTexture = new Texture2D(GraphicsDevice, 1, 1);
            _blockTexture.SetData(new[] { Color.White });

            try
            {
                _font = Content.Load<SpriteFont>("GameFont");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement de la police: {ex.Message}");
            }

            _renderer.Initialize(_spriteBatch, _blockTexture, _font);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            _inputHandler.HandleInput(deltaTime, _gameState);

            if (_gameState.IsGameOver)
            {
                base.Update(gameTime);
                return;
            }

            bool fastFall = _inputHandler.IsFastFalling();
            float fallSpeed = fastFall ? GameState.FastFallSpeed : _gameState.CurrentFallSpeed;

            _fallTimer += deltaTime;
            if (_fallTimer >= fallSpeed)
            {
                _fallTimer = 0;
                _gameState.Board.Update(_gameState.CurrentPiece);
                if (!_gameState.Board.CanPlacePiece(_gameState.CurrentPiece))
                {
                    int linesCleared = _gameState.Board.ClearFullLines();
                    _gameState.ProcessClearedLines(linesCleared);
                    _gameState.SpawnNextPiece();
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _renderer.Draw(_gameState);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}