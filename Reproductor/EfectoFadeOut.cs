﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace Reproductor
{
    class EfectoFadeOut : ISampleProvider
    {
        //Entrada
        private ISampleProvider fuente;
        private int muestrasLeidas = 0;
        private float segundosTranscurridos = 0;
        private float duracion;
        private float inicio;

        public EfectoFadeOut(ISampleProvider fuente, float duracion, float inicio)
        {
            this.fuente = fuente;
            this.duracion = duracion;
            this.inicio = inicio;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int read = fuente.Read(buffer, offset, count);
            //Proceso = Modificacion de los valores del buffer
            //Ap
            muestrasLeidas += read;
            segundosTranscurridos = (float)muestrasLeidas / (float)fuente.WaveFormat.SampleRate / (float)fuente.WaveFormat.Channels;

            if (segundosTranscurridos >= inicio)
            {
                //Aplicar el efecto - Salida=> La variable buffer modificada
                float factorEscala = 1- ((segundosTranscurridos - inicio) / duracion);
                for (int i = 0; i < read; i++)
                {
                    buffer[i + offset] *= factorEscala;
                }
                if(segundosTranscurridos >= (inicio+duracion))
                {
                    for(int i = 0; i < read; i ++)
                    {
                        buffer[i + offset] *= 0;
                    }                    
                }
            }

            return read;
        }
    }
}

