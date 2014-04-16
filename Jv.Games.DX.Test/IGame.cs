using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jv.Games.DX.Test
{
    public interface IGame
    {
		//Inicializa o jogo. Roda imediatamente antes do game loop iniciar.
		void Setup(SharpDX.Direct3D9.Device device);

		//Ativado sempre que um evento do windows chega.
		//void processEvent(const WindowsEvent& msg) = 0;

		//Chamado todo game loop, para que o jogo atualize sua lógica.
		//O time representa o tempo transocorrido, em segundos, desse frame 
		//em relação ao anterior.
		//
		//Deve retornar falso para o jogo acabar.
		bool Process(float time);

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
