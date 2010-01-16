////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM
//	�T���v���Q�[���p �R���e���c �v���Z�b�T
//		Copyright (c) 2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace danmaq.common_cpe.ball
{

	//* �������������������������������������������������������������������������� *
	/// <summary>
	/// This class will be instantiated by the XNA Framework Content Pipeline
	/// to apply custom processing to content data, converting an object of
	/// type TInput to TOutput. The input and output types may be the same if
	/// the processor wishes to alter data without changing its type.
	///
	/// This should be part of a Content Pipeline Extension Library project.
	///
	/// TODO: change the ContentProcessor attribute to specify the correct
	/// display name for this processor.
	/// </summary>
	[ContentProcessor(DisplayName = "�Ԃ��� ���� �����Q�[�� �X�v���C�g �t�H���g �v���Z�b�T")]
	public sealed class CCPEBallFont : FontTextureProcessor
	{

		//* �����������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
		//* constants ������������������������������������������������������������-*

		/// <summary>�����ꗗ�B</summary>
		private const string charList =
			" ()-/.0123456789:ACEFHIMOPRS" +
			"acdeghilmnqrstv�O�P�Q�R�S�T�U" +
			"�V�W�X�B�I�[���������������Ă�" +
			"�E�L�Q�X�}���Ջ��ʏ��ԍ��I��" +
			"�ő�x��A";

		//* ���������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
		//* methods ��������������������������������������������������������������-*

		//* -----------------------------------------------------------------------*
		/// <summary>�O���t �C���f�b�N�X�𕶎��Ƀ}�b�v���܂��B</summary>
		/// 
		/// <param name="index">�O���t �C���f�b�N�X�B</param>
		/// <returns>�w�肳�ꂽ�O���t �C���f�b�N�X�ŕ\���ꂽ�����B</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="index"/>���A�g�p�\�ȕ����͈̔͊O�ł��B
		/// </exception>
		protected override char GetCharacterForIndex(int index)
		{
			if(index >= charList.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return charList[index];
		}
	}
}
