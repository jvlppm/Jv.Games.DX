using System;
using System.Windows.Forms;

namespace Mage
{
    public interface IGame
    {
        //Inicializa o jogo. Roda imediatamente antes do game loop iniciar.
        void Setup(SharpDX.Direct3D9.Device device, GameWindow window);

        //Ativado sempre que um evento do windows chega.
        void ProcessEvent(Message msg);

        //Chamado todo game loop, para que o jogo atualize sua lógica.
        //O time representa o tempo transocorrido, em segundos, desse frame 
        //em relação ao anterior.
        //
        //Deve retornar falso para o jogo acabar.
        bool Process(SharpDX.Direct3D9.Device device, TimeSpan deltaTime);

        //Chamado todo game loop, para que o jogo desenhe a cena.
        void Paint(SharpDX.Direct3D9.Device device);

        //Chamado sempre que o Device é perdido.
        void OnLostDevice();

        //Chamado sempre que o Device é restaurado.
        void OnRestoreDevice(SharpDX.Direct3D9.Device device);

        //Finaliza o jogo. Roda após o game loop finalizar.
        void ShutDown(SharpDX.Direct3D9.Device device);
    }
}
