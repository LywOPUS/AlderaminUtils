using System.Collections.Generic;
using UnityEngine;

namespace AlderaminUtils
{
    class Unit
    {
        private int x;
        private int y;
        private Transform _trans;
        void MoveTo(Transform transform)
        {
            _trans = transform;
        }
    }

    public class CommandInvoker
    {
        public Queue<ICommand> CommandBuffer;

        public List<ICommand> CommandHistory;

        //current history index
        public int Cur;

        public CommandInvoker(Queue<ICommand> commandBuffer, List<ICommand> commandHistory)
        {
            CommandBuffer = commandBuffer;
            CommandHistory = commandHistory;
        }
    }

    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}